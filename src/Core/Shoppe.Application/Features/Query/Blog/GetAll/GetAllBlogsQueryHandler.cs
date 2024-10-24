using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.GetAll
{
    public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQueryRequest, GetAllBlogsQueryResponse>
    {
        public Task<GetAllBlogsQueryResponse> Handle(GetAllBlogsQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
