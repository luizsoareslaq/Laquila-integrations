using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            try
            {
                context.Request.EnableBuffering();

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
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