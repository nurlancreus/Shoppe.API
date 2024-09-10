using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Shoppe.API.Configurations
{
    public static class RateLimitingConfiguration
    {
        public static void ConfigureRateLimiting(this WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(options => options
          .AddFixedWindowLimiter(policyName: "fixed", limiterOptions =>
          {
              limiterOptions.PermitLimit = 100;
              limiterOptions.Window = TimeSpan.FromMinutes(1);
              limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
              limiterOptions.QueueLimit = 5;
          }));
        }
    }
}
