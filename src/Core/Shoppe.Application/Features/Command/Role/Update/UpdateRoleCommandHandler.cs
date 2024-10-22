using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Role.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            await _roleService.UpdateAsync(new DTOs.Role.UpdateRoleDTO
            {
                RoleId = request.RoleId!,
                Name = request.Name,
                Description = request.Description,
            }, cancellationToken);

            return new UpdateRoleCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
