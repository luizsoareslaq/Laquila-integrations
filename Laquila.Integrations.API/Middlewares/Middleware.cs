using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Models;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.API.Middlewares
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
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
            }

            stopwatch.Stop();
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            var statusCode = response.StatusCode;

            // _ = Task.Run(async () =>
            //                         {
            //                             using var scope = context.RequestServices.CreateScope();
            //                             var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
            //                             await logService.HandleLogAsync(new LaqApiLogs(
            //                                 Guid.TryParse(apiUserId, out var id) ? id : (Guid?)null,
            //                                 method,
            //                                 endpoint,
            //                                 queryString,
            //                                 requestBody,
            //                                 responseText,
            //                                 statusCode,
            //                                 ipAddress ?? "",
            //                                 userAgent,
            //                                 (int)stopwatch.ElapsedMilliseconds,
            //                                 DateTime.Now
            //                             ));

            //                         });

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
    }
}