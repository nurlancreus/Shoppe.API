using Microsoft.AspNetCore.Diagnostics;
using Shoppe.Domain.Exceptions.Base;
using System.Net.Mime;
using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Default status code

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        var problemDetails = CreateProblemDetails(exception, context.Request.Path);
                        
                        problemDetails.Status ??= (int)HttpStatusCode.InternalServerError;

                        try
                        {
                            // Log the error with all details, differentiating between critical and error logs
                            if (problemDetails.Status == (int)HttpStatusCode.InternalServerError)
                            {
                                logger.LogCritical(exception, "A critical error occurred: {Message}", problemDetails.Detail);
                            }
                            else
                            {
                                logger.LogError(exception, "An error occurred: {Message}", problemDetails.Detail);
                            }

                            context.Response.StatusCode = problemDetails.Status.Value;

                            // Write serialized problem details to the response
                            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                WriteIndented = true
                            }));
                        }
                        catch (Exception serializationException)
                        {
                            logger.LogError(serializationException, "Failed to serialize problem details.");
                            await context.Response.WriteAsync("An unexpected error occurred, and details could not be serialized.");
                        }
                    }
                });
            });
        }

        private static ProblemDetails CreateProblemDetails(Exception exception, string path)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Instance = path,
                Title = "An unexpected error occurred.",
                Status = (int)statusCode
            };

            switch (exception)
            {
                case BaseException baseException:
                    problemDetails.Status = (int)baseException.StatusCode;
                    problemDetails.Title = baseException.Title;
                    problemDetails.Detail = baseException.Message;
                    break;

                case ValidationException validationException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Validation failed.";

                    // Check if there are specific validation errors with properties
                    var errors = validationException.Errors.ToList();
                    if (errors.Any())
                    {
                        problemDetails.Extensions["errors"] = errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                    }
                    else
                    {
                        problemDetails.Detail = validationException.Message;
                    }
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Title = "Unauthorized access.";
                    problemDetails.Detail = unauthorizedAccessException.Message;
                    break;

                case ArgumentNullException argNullException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Invalid argument.";
                    problemDetails.Detail = argNullException.Message;
                    break;

                case ArgumentException argException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Invalid argument.";
                    problemDetails.Detail = argException.Message;
                    break;

                case NotImplementedException notImplementedException:
                    problemDetails.Status = (int)HttpStatusCode.NotImplemented;
                    problemDetails.Title = "Functionality not implemented.";
                    problemDetails.Detail = notImplementedException.Message;
                    break;

                case InvalidOperationException invalidOperationException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "This operation is not valid.";
                    problemDetails.Detail = invalidOperationException.Message;
                    break;

                default:
                    problemDetails.Detail = exception.Message;
                    break;
            }

            var innerExceptionMessages = GetInnerExceptionMessages(exception);
            if (innerExceptionMessages.Any())
            {
                problemDetails.Extensions["innerExceptions"] = innerExceptionMessages;
            }

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            return problemDetails;
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
