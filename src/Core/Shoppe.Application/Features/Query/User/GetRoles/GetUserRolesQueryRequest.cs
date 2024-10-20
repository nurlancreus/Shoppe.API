using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.GetRoles
{
    public class GetUserRolesQueryRequest : IRequest<GetUserRolesQueryResponse>
    {
        public string? UserId { get; set; }
    }
}
