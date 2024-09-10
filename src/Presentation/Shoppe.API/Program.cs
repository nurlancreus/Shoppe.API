
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

namespace Shoppe.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();

            builder.Services
                .RegisterApplicationServices()
                .RegisterPersistenceServices(builder.Configuration)
                .RegisterInfrastructureServices();

            builder.Services.AddStorage(StorageType.AWS, builder.Configuration);

            builder.ConfigureLogging();
            builder.ConfigureOptions();

            builder.Services.AddControllers();

            builder.ConfigureIdentity();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.ConfigureSwaggerGen();

            //Enable versioning in Web API controllers
            builder.ConfigureVersioning();

            // Implement built-in rate limiter
            builder.ConfigureRateLimiting();

            var app = builder.Build();

            //Use rate limiter
            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // should put above everything you want to log (only logs the things coming after itself)
            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            // for http logging
            app.UseHttpLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // should put after UseAuth* middlewares
            app.UseMyLoggingMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
