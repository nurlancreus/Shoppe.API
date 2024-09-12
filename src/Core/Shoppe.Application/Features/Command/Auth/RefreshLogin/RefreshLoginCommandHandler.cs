using MediatR;
using Shoppe.Application.Abstractions.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Auth.RefreshLogin
{
    public class RefreshLoginCommandHandler : IRequestHandler<RefreshLoginCommandRequest, RefreshLoginCommandResponse>
    {
        private readonly IAuthService _authService;

        public RefreshLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RefreshLoginCommandResponse> Handle(RefreshLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.RefreshTokenLoginAsync(request.AccessToken, request.RefreshToken);

            return new RefreshLoginCommandResponse()
            {
                IsSuccess = true,
                Token = token
            };
        }
    }
}
