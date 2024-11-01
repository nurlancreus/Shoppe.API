using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.About.RemoveImage
{
    public class RemoveImageCommandRequest : IRequest<RemoveImageCommandResponse>
    {
        public string? SectionId { get; set; }
        public string? ImageId { get; set; }
    }
}
