using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.Get
{
    public class GetReplyByIdQueryRequest : IRequest<GetReplyByIdQueryResponse>
    {
        public string? Id { get; set; }
    }
}
