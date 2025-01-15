using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mock.ShippingProvider.API.Middlewares;
using Mock.ShippingProvider.Application.Behaviours;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Interfaces.Services;
using Mock.ShippingProvider.Application.Options;
using Mock.ShippingProvider.Infrastructure.Persistence;
using Mock.ShippingProvider.Infrastructure.Persistence.Repositories;
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

            // Configure DbContext with Scoped lifetime
            builder.Services.AddDbContext<ShippingProviderDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Mock"), sqlOptions => sqlOptions
                .MigrationsAssembly(typeof(ShippingProviderDbContext).Assembly.FullName))
                .EnableSensitiveDataLogging();
            });

            // Register MediatR
            builder.Services.AddMediatR(config => {
                config.RegisterServicesFromAssembly(typeof(ValidationPipelineBehaviour<,>).Assembly);
                config.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
                config.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));

            });

            // Register Repositories
            #region Register Repositories
            builder.Services.AddScoped<IApiClientRepository, ApiClientRepository>();
            builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
            #endregion

            // Register Services
            #region Register Client Services
            builder.Services.AddScoped<IGeoInfoService, GeoInfoService>();
            builder.Services.AddScoped<ICalculatorService, CalculatorService>();
            #endregion


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register Options pattern
            builder.Services.Configure<APISettings>(APISettings.GeoCodeAPI,
                    builder.Configuration.GetSection("API:GeoCodeAPI"));
            builder.Services.Configure<ShippingRatesSettings>(builder.Configuration.GetSection("ShippingRatesSettings"));
        }

        public static void RegisterMiddlewares(this WebApplication app)
        {
            app.UseStatusCodePages(async statusCodeCntx
                    => await TypedResults.Problem(statusCode: statusCodeCntx.HttpContext.Response.StatusCode)
                 .ExecuteAsync(statusCodeCntx.HttpContext));


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();
        }
    }

}
