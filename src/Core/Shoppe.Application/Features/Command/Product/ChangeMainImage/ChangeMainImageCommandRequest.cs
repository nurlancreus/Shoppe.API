using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Product.ChangeMainImage
{
    public class ChangeMainImageCommandRequest : IRequest<ChangeMainImageCommandResponse>
    {
        public string? ProductId { get; set; }
        public string? ImageId { get; set; }
    }
}
