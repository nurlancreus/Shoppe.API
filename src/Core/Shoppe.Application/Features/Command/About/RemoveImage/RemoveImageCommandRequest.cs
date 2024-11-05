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
        public Guid? SectionId { get; set; }
        public Guid? ImageId { get; set; }
    }
}
