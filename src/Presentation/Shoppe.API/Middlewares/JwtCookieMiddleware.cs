namespace Shoppe.API.Middlewares
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["accessToken"];
            var headerToken = context.Request.Headers.Values;

            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }

            await _next(context);
        }
    }

}
