using MediatR;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Application.Constants;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(request.ToLoginRequestDTO());

            return new LoginCommandResponse()
            {
                IsSuccess = true,
                Token = token,
                Message = ResponseConst.LoginSuccessMessage()
            };
        }
    }
}
