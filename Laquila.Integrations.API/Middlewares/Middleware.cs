using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using Laquila.Integrations.API.Logging;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.API.Middlewares
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        static readonly SemaphoreSlim _fileLock = new(1, 1);
        static readonly ConcurrentDictionary<string, int> _dailyCount = new();
        static string TodayKey() => DateTime.UtcNow.ToString("yyyy_MM_dd", CultureInfo.InvariantCulture);
        protected readonly ErrorCollector errors = new ErrorCollector();
        private readonly IMemoryCache _cache;

        public Middleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Request.EnableBuffering(bufferThreshold: 1024 * 30); // até 30KB sem ir pra disco

            var request = context.Request;
            var response = context.Response;

            var method = request.Method;
            var endpoint = request.Path;
            var queryString = request.QueryString.ToString();
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers["User-Agent"].ToString();
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cnpj = context.User?.FindFirst("CompanyCnpj")?.Value;
            var language = context.User?.FindFirst("Language")?.Value;

            UserContext.CompanyCnpj = cnpj;
            UserContext.Language = language;

            string? entity = null;
            string? key = null;
            string? value = null;

            string requestBody;
            request.Body.Position = 0;
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            // if (requestBody.Length > 10_000)
            //     requestBody = "[Truncated Payload]";

            var requestSignature = GenerateHash($"{userId}-{method}-{endpoint}-{requestBody}");

            if (_cache.TryGetValue(requestSignature, out _))
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsync("Duplicate request detected. Please wait a few seconds.");
                UserContext.Clear();
                return;
            }
            else
            {
                _cache.Set(requestSignature, true, TimeSpan.FromSeconds(10));
            }

            var originalBodyStream = response.Body;
            using var responseBody = new MemoryStream();
            response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

            stopwatch.Stop();
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            var statusCode = response.StatusCode;

            if (statusCode == StatusCodes.Status429TooManyRequests)
            {
                var line = $"{DateTime.UtcNow:O}\t{ipAddress}\t{endpoint}\t{queryString}\t{userAgent}\t{statusCode}";
                await AppendSimpleLogAsync("ratelimit", line, dailyLimit: 10000);

                response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                UserContext.Clear();
                return;
            }

            using var scope = context.RequestServices.CreateScope();
            var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

            response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            await LogChannel.Channel.Writer.WriteAsync(new LaqApiLogs(
                Guid.TryParse(userId, out var id) ? id : (Guid?)null,
                method,
                endpoint,
                queryString,
                requestBody,
                responseText,
                statusCode,
                ipAddress ?? "",
                userAgent,
                (int)stopwatch.ElapsedMilliseconds,
                DateTime.Now,
                entity,
                key,
                value
            ));

            UserContext.Clear();
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message += " InnerException: " + ex.InnerException.Message;
            }

            var response = new ResponseDto
            {
                Data = null,
                Errors = new List<ResponseErrorsDto>()
            };

            if (ex is ResponseErrorException responseErrors)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                response.Errors = responseErrors.Errors.Select(e => new ResponseErrorsDto
                {
                    StatusCode = e.StatusCode,
                    Entity = e.Entity,
                    Key = e.Key,
                    Value = e.Value,
                    Message = e.Message
                }).ToList();

                return context.Response.WriteAsJsonAsync(response);
            }

            (HttpStatusCode status, string responseMessage) = ex switch
            {
                CustomErrorException custom => ((HttpStatusCode)custom.StatusCode, message),
                NotFoundException => (HttpStatusCode.NotFound, message),
                BadRequestException => (HttpStatusCode.BadRequest, message),
                InvalidOperationException => (HttpStatusCode.BadRequest, "Operação inválida: " + message),
                UnauthorizedError => (HttpStatusCode.Unauthorized, message),
                Exception => (HttpStatusCode.BadRequest, message),
                _ => (HttpStatusCode.InternalServerError, "Erro interno: " + message)
            };

            context.Response.StatusCode = (int)status;

            response.Errors.Add(new ResponseErrorsDto
            {
                StatusCode = (int)status,
                Entity = string.Empty,
                Key = string.Empty,
                Value = string.Empty,
                Message = responseMessage
            });

            return context.Response.WriteAsJsonAsync(response);
        }

        static async Task AppendSimpleLogAsync(string filePrefix, string line, int dailyLimit)
        {
            var key = $"{filePrefix}-{TodayKey()}";
            var count = _dailyCount.AddOrUpdate(key, 0, (_, c) => c);

            if (count >= dailyLimit) return;

            await _fileLock.WaitAsync();
            try
            {
                // revalida com lock (evita corrida)
                count = _dailyCount.AddOrUpdate(key, 0, (_, c) => c);
                if (count >= dailyLimit) return;

                var dir = Path.Combine(AppContext.BaseDirectory, "logs");
                Directory.CreateDirectory(dir);

                var filePath = Path.Combine(dir, $"{filePrefix}-{TodayKey()}.txt");
                await File.AppendAllTextAsync(filePath, line + Environment.NewLine);

                _dailyCount.AddOrUpdate(key, 1, (_, c) => c + 1);
            }
            finally { _fileLock.Release(); }
        }

        private string GenerateHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}