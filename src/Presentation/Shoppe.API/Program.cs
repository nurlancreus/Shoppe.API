
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using Shoppe.API.Configurations;
using Shoppe.API.Middlewares;
using Shoppe.Persistence;
using System.Threading.RateLimiting;
using Shoppe.Infrastructure;
using Shoppe.Application;
using Shoppe.Domain.Enums;
using FluentValidation.AspNetCore;
using FluentValidation;
using Shoppe.Application.Validators.Product;
using Shoppe.SignalR;

namespace Shoppe.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();

            builder.Services
                .RegisterApplicationServices()
                .RegisterPersistenceServices(builder.Configuration)
                .RegisterInfrastructureServices()
                .RegisterSignalRServices();

            builder.Services.AddStorage(StorageType.AWS, builder.Configuration);

            builder.ConfigureLogging();
            builder.ConfigureOptions();
            builder.ConfigureCors();

            builder.Services//AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

                                                        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandRequestValidator>();

            builder.Services.AddControllers();

            builder.ConfigureIdentity();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.ConfigureSwaggerGen();

            //Enable versioning in Web API controllers
            builder.ConfigureVersioning();

            // Implement built-in rate limiter
            builder.ConfigureRateLimiting();

            builder.ConfigureAuth();

            var app = builder.Build();
            app.UseCors("AllowShoppeClient");

            //Use rate limiter
            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

            // should put above everything you want to log (only logs the things coming after itself)
            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            // for http logging
            app.UseHttpLogging();

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMyJwtCookieMiddleware();
            app.UseAuthorization();

            // should put after UseAuth* middlewares
            app.UseMyLoggingMiddleware();

            app.MapControllers();

            // Map SignalR Hubs
            app.MapHubs();

            app.Run();
        }
    }
}
