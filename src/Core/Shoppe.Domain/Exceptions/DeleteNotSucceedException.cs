using Shoppe.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    public class DeleteNotSucceedException : BaseException
    {
        public override string Title => nameof(DeleteNotSucceedException);
        public DeleteNotSucceedException()
            : base("The entity could not be deleted.", HttpStatusCode.BadRequest)
        {
        }

        public DeleteNotSucceedException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public DeleteNotSucceedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
