using Serilog.Context;

namespace Shoppe.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var username = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;

            // Push property to the log context for the duration of this request
            using (LogContext.PushProperty("user_name", username))
            {
                await _next(context);
            }
        }
    }

    public static class CustomMiddlewareExtension
    {
        public static void UseMyLoggingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<LoggingMiddleware>();
        }
    }
}
