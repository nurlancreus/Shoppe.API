using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reaction.GetBlogReactions
{
    public class GetBlogReactionsQueryRequest : IRequest<GetBlogReactionsQueryResponse>
    {
        public Guid BlogId { get; set; }
    }
}
