namespace Shoppe.API.Middlewares
{
    public static class UseMiddlewares
    {
        public static void UseMyLoggingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<LoggingMiddleware>();
        }

        public static void UseMyJwtCookieMiddleware(this WebApplication app)
        {
            app.UseMiddleware<JwtCookieMiddleware>();
        }
    }
}
