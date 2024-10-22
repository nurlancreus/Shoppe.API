using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.Deactivate
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommandRequest, DeactivateUserCommandResponse>
    {
        private readonly IUserService _userService;

        public DeactivateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<DeactivateUserCommandResponse> Handle(DeactivateUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.DeactivateAsync(request.UserId!, cancellationToken);

            return new DeactivateUserCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
