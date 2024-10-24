using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.Get
{
    public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQueryRequest, GetBlogByIdQueryResponse>
    {
        public Task<GetBlogByIdQueryResponse> Handle(GetBlogByIdQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
