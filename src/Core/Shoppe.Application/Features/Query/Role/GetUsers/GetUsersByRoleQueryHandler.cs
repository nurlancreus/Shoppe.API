using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Role.GetUsers
{
    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQueryRequest, GetUsersByRoleQueryResponse>
    {
        public Task<GetUsersByRoleQueryResponse> Handle(GetUsersByRoleQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
