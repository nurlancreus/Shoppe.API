using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Role.AssignUsers
{
    public class AssignUsersToRoleCommandHandler : IRequestHandler<AssignUsersToRoleCommandRequest, AssignUsersToRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public AssignUsersToRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<AssignUsersToRoleCommandResponse> Handle(AssignUsersToRoleCommandRequest request, CancellationToken cancellationToken)
        {
            await _roleService.AssignUsersToRoleAsync(request.RoleId!, request.UserNames, cancellationToken);

            return new AssignUsersToRoleCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
