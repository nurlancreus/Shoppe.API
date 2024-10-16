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
        public string? ProductId { get; set; }
        public string? ImageId { get; set; }
    }
}
