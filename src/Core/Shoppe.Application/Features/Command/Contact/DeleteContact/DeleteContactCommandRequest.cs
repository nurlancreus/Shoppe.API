using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.DeleteContact
{
    public class DeleteContactCommandRequest : IRequest<DeleteContactCommandResponse>
    {
        public string? Id { get; set; }
    }
}
