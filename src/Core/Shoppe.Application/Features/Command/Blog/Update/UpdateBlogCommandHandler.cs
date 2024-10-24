using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Update
{
    public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommandRequest, UpdateBlogCommandResponse>
    {
        public Task<UpdateBlogCommandResponse> Handle(UpdateBlogCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
