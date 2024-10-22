using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Role.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public CreateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            await _roleService.CreateAsync(new CreateRoleDTO
            {
                Name = request.Name,
                Description = request.Description,
            }, cancellationToken);

            return new CreateRoleCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
