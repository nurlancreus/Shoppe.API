using Shoppe.Domain.Exceptions.Base;
using System;
using System.Net;

namespace Shoppe.Domain.Exceptions
{
    public enum AuthErrorType
    {
        Register,
        Login,
        Authorize
    }

    public class AuthException : BaseException
    {
        public override string Title => nameof(AuthException);
        public AuthErrorType ErrorType { get; }

        // Modify the constructor to set status code based on error type
        public AuthException(AuthErrorType errorType)
            : base(GenerateMessage(errorType), DetermineStatusCode(errorType))
        {
            ErrorType = errorType;
        }

        public AuthException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        // Generate message based on error type
        private static string GenerateMessage(AuthErrorType errorType)
        {
            return errorType switch
            {
                AuthErrorType.Register => "Cannot register user. Wrong credentials",
                AuthErrorType.Login => "Cannot login user. Wrong credentials",
                AuthErrorType.Authorize => "User is not authorized",
                _ => "Wrong credentials"
            };
        }

        // Determine status code based on error type
        private static HttpStatusCode DetermineStatusCode(AuthErrorType errorType)
        {
            return errorType switch
            {
                AuthErrorType.Authorize => HttpStatusCode.Unauthorized, // 401 for unauthorized
                _ => HttpStatusCode.BadRequest // 400 for register or login errors
            };
        }
    }
}
