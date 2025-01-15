using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.API.Attributes;
using Mock.ShippingProvider.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace Mock.ShippingProvider.API.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ShippingProviderDbContext dbContext)
        {
            // Check if the endpoint has the AllowAnonymousApiKey attribute
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousApiKeyAttribute>() != null;

            if (allowAnonymous)
            {
                await _next(context); // Bypass authentication
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey) ||
                !context.Request.Headers.TryGetValue("X-Secret-Key", out var extractedSecretKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key or Secret Key is missing.");
                return;
            }

            var client = await dbContext.ApiClients
                .FirstOrDefaultAsync(a => a.ApiKey == extractedApiKey && a.IsActive);

            if (client == null || string.IsNullOrEmpty(extractedSecretKey) || !VerifySecretKey(extractedSecretKey!, client.SecretKey))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Invalid API Key or Secret Key.");
                return;
            }

            await _next(context);
        }

        private static bool VerifySecretKey(string providedSecret, string storedSecret)
        {
            // Use HMACSHA256 for secret key comparison
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(storedSecret));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(providedSecret));
            return storedSecret == Convert.ToBase64String(computedHash);
        }
    }
}
