using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.RemoveImage
{
    public class RemoveBlogImageCommandRequest : IRequest<RemoveBlogImageCommandResponse>
    {
        public Guid? BlogId { get; set; }
        public Guid? ImageId { get; set; }
    }
}
