using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.GetRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQueryRequest, GetUserRolesQueryResponse>
    {
        public Task<GetUserRolesQueryResponse> Handle(GetUserRolesQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
