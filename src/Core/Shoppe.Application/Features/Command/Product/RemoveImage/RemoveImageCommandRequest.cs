using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Product.RemoveImage
{
    public class RemoveImageCommandRequest : IRequest<RemoveImageCommandResponse>
    {
        public Guid? ProductId { get; set; }
        public Guid? ImageId { get; set; }
    }
}
