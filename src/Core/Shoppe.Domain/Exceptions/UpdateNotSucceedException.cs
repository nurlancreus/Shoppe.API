using Shoppe.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    public class UpdateNotSucceedException : BaseException
    {
        public override string Title => nameof(UpdateNotSucceedException);
        public UpdateNotSucceedException()
            : base("The entity could not be Updated.", HttpStatusCode.BadRequest)
        {
        }

        public UpdateNotSucceedException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public UpdateNotSucceedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
