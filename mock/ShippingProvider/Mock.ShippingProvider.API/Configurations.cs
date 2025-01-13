using Mock.ShippingProvider.Application.Interfaces;
using Mock.ShippingProvider.Application.Options;
using Mock.ShippingProvider.Infrastructure.Services;

namespace Mock.ShippingProvider.API
{
    public static class Configurations
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddHttpClient();

            #region Register Business
            builder.Services.AddScoped<IGeoInfoService, GeoInfoService>();
            #endregion

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<APIOptions>(APIOptions.GeoCodeAPI,
                    builder.Configuration.GetSection("API:GeoCodeAPI"));
        }

        public static void RegisterMiddlewares(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
        }
    }

}
