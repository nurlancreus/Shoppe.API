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

        public async Task<RemoveBlogImageCommandResponse> Handle(RemoveBlogImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _blogService.RemoveImageAsync((Guid)request.BlogId!, (Guid)request.ImageId!, cancellationToken);

            return new RemoveBlogImageCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
