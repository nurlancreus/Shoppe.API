using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Role.Get
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQueryRequest, GetRoleQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetRoleQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRoleQueryResponse> Handle(GetRoleQueryRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetAsync(request.RoleId!, cancellationToken);

            return new GetRoleQueryResponse
            {
                IsSuccess = true,
                Data = role,
            };
        }
    }
}
