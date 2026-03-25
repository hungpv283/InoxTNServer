using InoxServer.Domain.Errors;
using System.Net;
using System.Text.Json;

namespace InoxServer.Presentation.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await HandleDomainExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleUnexpectedExceptionAsync(context);
            }
        }

        private static Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.Error.StatusCode;

            var response = new
            {
                code = ex.Error.Code,
                message = ex.Error.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleUnexpectedExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                code = "Server.InternalError",
                message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
