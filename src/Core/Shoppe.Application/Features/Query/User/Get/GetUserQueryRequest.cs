using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.Get
{
    public class GetUserQueryRequest : IRequest<GetUserQueryResponse>
    {
        public string? UserId { get; set; }
    }
}
