using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reaction.GetReplyReactions
{
    public class GetReplyReactionsQueryRequest : IRequest<GetReplyReactionsQueryResponse>
    {
        public Guid ReplyId { get; set; }
    }
}
