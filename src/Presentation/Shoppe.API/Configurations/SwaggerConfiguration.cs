namespace Shoppe.API.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwaggerGen (this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });

                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "2.0" });

            }); //generates OpenAPI specification
        }
    }
}
