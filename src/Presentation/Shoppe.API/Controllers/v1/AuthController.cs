using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Auth.Login;
using Shoppe.Application.Features.Command.Auth.RefreshLogin;
using Shoppe.Application.Features.Command.Auth.Register;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class AuthController : ApplicationControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterCommandRequest registerCommandRequest)
        {
            var response = await _mediator.Send(registerCommandRequest);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginCommandRequest loginCommandRequest)
        {
            var response = await _mediator.Send(loginCommandRequest);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshLogin(RefreshLoginCommandRequest refreshLoginCommandRequest)
        {
            var response = await _mediator.Send(refreshLoginCommandRequest);

            return Ok(response);
        }
    }
}
