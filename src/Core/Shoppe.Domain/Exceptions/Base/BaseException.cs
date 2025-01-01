using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions.Base
{
    public abstract class BaseException : Exception
    {
        public virtual string Title { get; set; } = string.Empty;
        public virtual string Description { get; set;} = string.Empty;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

        protected BaseException()
        {
            
        }
        protected BaseException(string message) : base(message)
        {

        }

        protected BaseException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        protected BaseException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        protected BaseException(string? message, HttpStatusCode statusCode, Exception? innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        protected BaseException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
