using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.GetAll
{
    public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQueryRequest, GetAllBlogsQueryResponse>
    {
        private readonly IBlogService _blogService;

        public GetAllBlogsQueryHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<GetAllBlogsQueryResponse> Handle(GetAllBlogsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _blogService.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllBlogsQueryResponse
            {
                IsSuccess = true,
                PageSize = result.PageSize,
                Page = result.Page,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Blogs
            };
        }
    }
}
