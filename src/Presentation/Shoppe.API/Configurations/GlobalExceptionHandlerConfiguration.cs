using Microsoft.AspNetCore.Diagnostics;
using Shoppe.Domain.Exceptions.Base;
using System.Net.Mime;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Shoppe.API.Configurations
{
    public static class GlobalExceptionHandlerConfiguration
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        HttpStatusCode statusCode;
                        string message;
                        string track = string.Empty;

                        if (exception is BaseException baseException)
                        {
                            statusCode = baseException.StatusCode;
                            message = baseException.Message;
                        }
                        else if (exception is ValidationException validationException)
                        {
                            statusCode = HttpStatusCode.BadRequest;
                            message = "Validation failed: " + string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage));
                        }
                        else
                        {
                            statusCode = HttpStatusCode.InternalServerError;
                            message = exception.Message;
                            track = exception.StackTrace ?? "";
                        }

                        // Collect all inner exceptions
                        var innerExceptionMessages = GetInnerExceptionMessages(exception);
                        if (innerExceptionMessages.Count > 0)
                        {
                            message += " | Inner Exceptions: " + string.Join(" | ", innerExceptionMessages);
                        }

                        // Log the error with inner exceptions
                        logger.LogError(exception, message);

                        context.Response.StatusCode = (int)statusCode;
                        var response = new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = message,
                            Title = "Exception caught!",
                            Track = track,
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }

        private static List<string> GetInnerExceptionMessages(Exception exception)
        {
            var messages = new List<string>();
            var currentException = exception.InnerException;

            while (currentException != null)
            {
                messages.Add(currentException.Message);
                currentException = currentException.InnerException;
            }

            return messages;
        }
    }
}
