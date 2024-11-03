using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.Deactivate
{
    public class ToggleUserCommandHandler : IRequestHandler<ToggleUserCommandRequest, ToggleUserCommandResponse>
    {
        private readonly IUserService _userService;

        public ToggleUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ToggleUserCommandResponse> Handle(ToggleUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.ToggleUserAsync(request.UserId!, cancellationToken);

            return new ToggleUserCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
