using Shoppe.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    public class InvalidIdException : BaseException
    {
        public override string Title => nameof(InvalidIdException);
        public InvalidIdException()
            : base("Invalid Id.", HttpStatusCode.BadRequest)
        {
        }

        public InvalidIdException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public InvalidIdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
