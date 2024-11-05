using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.Get
{
    public class GetBlogByIdQueryRequest : IRequest<GetBlogByIdQueryResponse>
    {
        public Guid? BlogId { get; set; }
    }
}
