using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _userService.UpdateAsync(request.ToUpdateUserDTO(), cancellationToken);

            return new UpdateUserCommandResponse
            {
                IsSuccess = true,
                Token = token,
        };
    }
}
}
