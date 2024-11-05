using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Tag.Delete
{
    public class DeleteTagCommandRequest : IRequest<DeleteTagCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
