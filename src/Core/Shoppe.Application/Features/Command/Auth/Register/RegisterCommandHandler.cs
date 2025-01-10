using MediatR;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Domain.Constants;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.RegisterAsync(request.ToRegisterRequestDTO());

            return new RegisterCommandResponse()
            {
                IsSuccess = true,
                Token = token,
                Message = AuthConst.RegisterSuccessMessage,
            };
        }
    }
}
