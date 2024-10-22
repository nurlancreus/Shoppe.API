using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.ChangeProfilePicture
{
    public class ChangeProfilePictureCommandHandler : IRequestHandler<ChangeProfilePictureCommandRequest, ChangeProfilePictureCommandResponse>
    {
        private readonly IUserService _userService;

        public ChangeProfilePictureCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ChangeProfilePictureCommandResponse> Handle(ChangeProfilePictureCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.ChangeProfilePictureAsync(request.UserId!, request.NewProfilePictureId!, cancellationToken);

            return new ChangeProfilePictureCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
