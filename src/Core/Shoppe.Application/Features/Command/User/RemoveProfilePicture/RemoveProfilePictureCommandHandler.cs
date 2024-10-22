using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.RemoveProfilePicture
{
    public class RemoveProfilePictureCommandHandler : IRequestHandler<RemoveProfilePictureCommandRequest, RemoveProfilePictureCommandResponse>
    {
        private readonly IUserService _userService;

        public RemoveProfilePictureCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RemoveProfilePictureCommandResponse> Handle(RemoveProfilePictureCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.RemovePictureAsync(request.UserId!, request.PictureId!, cancellationToken);

            return new RemoveProfilePictureCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
