using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Auth.Login;
using Shoppe.Application.Features.Command.Auth.RefreshLogin;
using Shoppe.Application.Features.Command.Auth.Register;

namespace Shoppe.API.Controllers.v1
{
    [AllowAnonymous]
    public class AuthController : ApplicationVersionController
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterCommandRequest registerCommandRequest)
        {
            var response = await _sender.Send(registerCommandRequest);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginCommandRequest loginCommandRequest)
        {
            var response = await _sender.Send(loginCommandRequest);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshLogin(RefreshLoginCommandRequest refreshLoginCommandRequest)
        {
            var response = await _sender.Send(refreshLoginCommandRequest);

            return Ok(response);
        }
    }
}
