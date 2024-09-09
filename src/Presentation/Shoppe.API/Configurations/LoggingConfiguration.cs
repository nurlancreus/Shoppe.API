using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Shoppe.API.Configurations
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            // Register IHttpContextAccessor for the UsernameEnricher
            builder.Services.AddHttpContextAccessor();

            // Create and configure the logger with the UsernameEnricher
            Logger log = new LoggerConfiguration()
                .Enrich.With(new UsernameEnricher(builder.Services.BuildServiceProvider().GetService<IHttpContextAccessor>()))
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt")
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("Default"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

            // Apply Serilog to the application
            builder.Host.UseSerilog(log);

            // Configure HTTP logging
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
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
