using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.GetRepliesByUser
{
    public class GetRepliesByUserQueryRequest : IRequest<GetRepliesByUserQueryResponse>
    {
        public string? UserId { get; set; }
    }
}
