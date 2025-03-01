using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Role.AssignUsers;
using Shoppe.Application.Features.Command.Role.Create;
using Shoppe.Application.Features.Command.Role.Delete;
using Shoppe.Application.Features.Command.Role.Update;
using Shoppe.Application.Features.Query.Role.Get;
using Shoppe.Application.Features.Query.Role.GetAll;
using Shoppe.Application.Features.Query.Role.GetUsers;

namespace Shoppe.API.Controllers.v1
{
    [Authorize(ApiConstants.AuthPolicies.SuperAdminPolicy)]
    public class RolesController : ApplicationVersionController
    {
        private readonly ISender _sender;

        public RolesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var request = new GetRoleQueryRequest { RoleId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var request = new GetAllRolesQueryRequest();
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRoleCommandRequest request)
        {
            request.RoleId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteRoleCommandRequest { RoleId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsers(string id)
        {
            var request = new GetUsersByRoleQueryRequest { RoleId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}/users")]
        public async Task<IActionResult> AssignUser([FromRoute] string id, [FromBody] AssignUsersToRoleCommandRequest request)
        {
            request.RoleId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
