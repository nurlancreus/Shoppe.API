using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Delete
{
    public class DeleteBlogCommandRequest : IRequest<DeleteBlogCommandResponse>
    {
        public Guid? BlogId { get; set; }
    }
}
