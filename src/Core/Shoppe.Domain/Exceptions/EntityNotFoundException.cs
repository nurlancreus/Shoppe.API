using Shoppe.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    public class EntityNotFoundException : BaseException
    {
        public override string Title => nameof(EntityNotFoundException);

        public string? EntityName { get; }
        public string? EntityId { get; }

        public EntityNotFoundException(string entityName)
            : base($"The entity '{entityName}' was not found.", HttpStatusCode.NotFound)
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, string entityId)
            : base($"The entity '{entityName}' with ID '{entityId}' was not found.", HttpStatusCode.NotFound)
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
