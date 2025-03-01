using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Microsoft.EntityFrameworkCore;
using Shoppe.Persistence.Context;

namespace Shoppe.API.Configurations
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            bool isDbAvailable = ShoppeDbContext.CheckDatabaseAvailability(builder.Configuration);

            // Create Logger Configuration
            var logConfig = new LoggerConfiguration()
                .Enrich.With(new UsernameEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>()))
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .MinimumLevel.Information();

            // If DB is available, enable MSSQL logging
            if (isDbAvailable)
            {
                logConfig.WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("Default"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information);
            }

            // Apply Serilog to the application
            Log.Logger = logConfig.CreateLogger();
            builder.Host.UseSerilog();

            // Configure HTTP logging only if DB is available
            if (isDbAvailable)
            {
                builder.Services.AddHttpLogging(logging =>
                {
                    logging.LoggingFields = HttpLoggingFields.All;
                    logging.RequestHeaders.Add("sec-ch-ua");
                    logging.MediaTypeOptions.AddText("application/javascript");
                    logging.RequestBodyLogLimit = 4096;
                    logging.ResponseBodyLogLimit = 4096;
                });
            }
        }

        public class UsernameEnricher : ILogEventEnricher
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UsernameEnricher(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";
                var userProperty = propertyFactory.CreateProperty("user_name", username);
                logEvent.AddPropertyIfAbsent(userProperty);
            }
        }
    }
}
