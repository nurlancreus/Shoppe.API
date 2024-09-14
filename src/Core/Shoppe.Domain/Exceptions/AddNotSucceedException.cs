using Shoppe.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    public class AddNotSucceedException : BaseException
    {
        public override string Title => nameof(AddNotSucceedException);
        public AddNotSucceedException()
            : base("The entity could not be added.", HttpStatusCode.BadRequest)
        {
        }

        public AddNotSucceedException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public AddNotSucceedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
