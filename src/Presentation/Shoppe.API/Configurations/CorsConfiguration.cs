namespace Shoppe.API.Configurations
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowShoppeClient", builder =>
                {
                    //builder.WithOrigins("http://localhost:3000/") // Add allowed origins
                    //       .AllowAnyHeader()
                    //       .AllowAnyMethod()
                    //       .AllowCredentials(); // Allow cookies, authorization headers, etc.

                    builder.AllowAnyOrigin();
                });
            });
        }
    }
}
