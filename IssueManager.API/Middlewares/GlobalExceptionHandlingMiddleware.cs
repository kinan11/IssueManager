using System.Net;
using System.Text.Json;

namespace IssueManager.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = "Unexpected server error";

            if (exception is InvalidOperationException)
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            context.Response.StatusCode = (int)status;

            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}
