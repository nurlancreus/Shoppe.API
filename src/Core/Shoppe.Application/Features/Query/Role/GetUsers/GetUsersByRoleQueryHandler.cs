using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Role.GetUsers
{
    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQueryRequest, GetUsersByRoleQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetUsersByRoleQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetUsersByRoleQueryResponse> Handle(GetUsersByRoleQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetUsersAsync(request.RoleId!, request.Page, request.PageSize, cancellationToken);

            return new GetUsersByRoleQueryResponse
            {
                IsSuccess = true,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Users,
            };
        }
    }
}
