using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Delete
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommandRequest, DeleteBlogCommandResponse>
    {
        private readonly IBlogService _blogService;

        public DeleteBlogCommandHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<DeleteBlogCommandResponse> Handle(DeleteBlogCommandRequest request, CancellationToken cancellationToken)
        {
            await _blogService.DeleteAsync((Guid)request.BlogId!, cancellationToken);

            return new DeleteBlogCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
