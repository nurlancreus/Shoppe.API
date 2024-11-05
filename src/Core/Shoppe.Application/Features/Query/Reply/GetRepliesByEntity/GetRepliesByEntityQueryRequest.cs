using MediatR;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.GetRepliesByEntity
{
    public class GetRepliesByEntityQueryRequest : IRequest<GetRepliesByEntityQueryResponse>
    {
        public Guid? EntityId { get; set; }
        public ReplyType ReplyType { get; set; } = ReplyType.Blog;
    }
}
