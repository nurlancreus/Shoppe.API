using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Role.Get
{
    public class GetRoleQueryRequest : IRequest<GetRoleQueryResponse>
    {
        public string? RoleId { get; set; }
    }
}
