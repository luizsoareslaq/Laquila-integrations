using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text;
using Laquila.Integrations.Application.Interfaces;
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


        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Request.EnableBuffering();

            var request = context.Request;
            var response = context.Response;

            var method = request.Method;
            var endpoint = request.Path;
            var queryString = request.QueryString.ToString();
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers["User-Agent"].ToString();
            var apiUserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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

                if (ex is CustomErrorException custom)
                {
                    if (!string.IsNullOrWhiteSpace(custom.Entity))
                    {
                        entity = custom.Entity;
                    }

                    if (!string.IsNullOrWhiteSpace(custom.Key))
                    {
                        key = custom.Key;
                    }

                    if (!string.IsNullOrWhiteSpace(custom.Value))
                    {
                        value = custom.Value;
                    }
                }
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
                return;
            }

            using var scope = context.RequestServices.CreateScope();
            var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
            await logService.HandleLogAsync(new LaqApiLogs(
                Guid.TryParse(apiUserId, out var id) ? id : (Guid?)null,
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

            response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            string message = ex.Message;
            if (ex.InnerException != null)
            {
                message += " InnerException: " + ex.InnerException.Message;
            }

            (HttpStatusCode status, string responseMessage) = ex switch
            {
                CustomErrorException custom => ((HttpStatusCode)custom.StatusCode, message),
                NotFoundException => (HttpStatusCode.NotFound, message),
                BadRequestException => (HttpStatusCode.BadRequest, message),
                InvalidOperationException => (HttpStatusCode.BadRequest, "Operação inválida: " + message),
                UnauthorizedError => (HttpStatusCode.Unauthorized, message),
                Exception => (HttpStatusCode.BadRequest, message),
                _ => (HttpStatusCode.InternalServerError, "Erro interno " + message)
            };

            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsJsonAsync(new { Erro = responseMessage });
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
    }
}