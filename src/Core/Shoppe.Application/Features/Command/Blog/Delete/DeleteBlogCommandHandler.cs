using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Delete
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommandRequest, DeleteBlogCommandResponse>
    {
        public Task<DeleteBlogCommandResponse> Handle(DeleteBlogCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
