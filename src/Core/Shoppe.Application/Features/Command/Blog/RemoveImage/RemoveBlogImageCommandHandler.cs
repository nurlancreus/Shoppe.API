using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.RemoveImage
{
    public class RemoveBlogImageCommandHandler : IRequestHandler<RemoveBlogImageCommandRequest, RemoveBlogImageCommandResponse>
    {
        private readonly IBlogService _blogService;

        public RemoveBlogImageCommandHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public Task<RemoveBlogImageCommandResponse> Handle(RemoveBlogImageCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
