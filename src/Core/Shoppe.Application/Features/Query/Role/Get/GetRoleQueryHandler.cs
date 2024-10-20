using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Role.Get
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQueryRequest, GetRoleQueryResponse>
    {
        public Task<GetRoleQueryResponse> Handle(GetRoleQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
