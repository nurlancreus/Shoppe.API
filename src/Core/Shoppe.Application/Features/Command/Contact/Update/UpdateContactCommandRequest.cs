using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.UpdateContact
{
    public class UpdateContactCommandRequest : IRequest<UpdateContactCommandResponse>
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
