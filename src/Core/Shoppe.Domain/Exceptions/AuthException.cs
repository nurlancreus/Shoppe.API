using Shoppe.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    public enum AuthErrorType
    {
        Register,
        Login
    }
    public class AuthException : BaseException
    {
        public override string Title => nameof(AuthException);
        public AuthErrorType ErrorType { get; }
        public AuthException(AuthErrorType errorType)
             : base(GenerateMessage(errorType), HttpStatusCode.BadRequest)
        {
            ErrorType = errorType;
        }

        public AuthException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        private static string GenerateMessage(AuthErrorType errorType)
        {
            return errorType switch
            {
                AuthErrorType.Register => "Cannot register user. Wrong credentials",
                AuthErrorType.Login => "Cannot login user. Wrong credentials",
                _ => "Wrong credentials"
            };
        }
    }
}
