using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.Get
{
    public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQueryRequest, GetBlogByIdQueryResponse>
    {
        private readonly IBlogService _blogService;

        public GetBlogByIdQueryHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<GetBlogByIdQueryResponse> Handle(GetBlogByIdQueryRequest request, CancellationToken cancellationToken)
        {
           var blog = await _blogService.GetAsync((Guid)request.BlogId!, cancellationToken);

            return new GetBlogByIdQueryResponse
            {
                IsSuccess = true,
                Data = blog,
            };
        }
    }
}
