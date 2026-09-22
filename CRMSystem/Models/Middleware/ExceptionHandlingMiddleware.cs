using System.Net;
using System.Text.Json;
using CRMSystem.Models.Responses;

namespace CRMSystem.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unhandled exception occurred while processing the request.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,

                InvalidOperationException => (int)HttpStatusCode.Conflict,

                UnauthorizedAccessException => (int)HttpStatusCode.Forbidden,

                _ => (int)HttpStatusCode.InternalServerError
            };

            var message = exception switch
            {
                KeyNotFoundException =>
                    exception.Message,

                InvalidOperationException =>
                    exception.Message,

                UnauthorizedAccessException =>
                    exception.Message,

                _ =>
                    "An unexpected error occurred."
            };

            var response = new ErrorResponse
            {
                Success = false,
                Message = message,
                Data = null
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}