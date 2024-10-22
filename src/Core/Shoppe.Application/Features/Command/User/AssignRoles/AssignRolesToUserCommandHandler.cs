using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.AssignRoles
{
    public class AssignRolesToUserCommandHandler : IRequestHandler<AssignRolesToUserCommandRequest, AssignRolesToUserCommandResponse>
    {
        private readonly IUserService _userService;

        public AssignRolesToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AssignRolesToUserCommandResponse> Handle(AssignRolesToUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.AssignRolesAsync(request.UserId!, request.Roles, cancellationToken);

            return new AssignRolesToUserCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
