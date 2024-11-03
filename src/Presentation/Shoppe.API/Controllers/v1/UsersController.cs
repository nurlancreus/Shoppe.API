using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.User.AssignRoles;
using Shoppe.Application.Features.Command.User.ChangeProfilePicture;
using Shoppe.Application.Features.Command.User.Deactivate;
using Shoppe.Application.Features.Command.User.Delete;
using Shoppe.Application.Features.Command.User.RemoveProfilePicture;
using Shoppe.Application.Features.Command.User.Update;
using Shoppe.Application.Features.Query.Reply.GetRepliesByUser;
using Shoppe.Application.Features.Query.Review.GetReviewByUser;
using Shoppe.Application.Features.Query.User.Get;
using Shoppe.Application.Features.Query.User.GetAll;
using Shoppe.Application.Features.Query.User.GetRoles;
using Shoppe.Application.Features.Query.User.GetUserPictures;

namespace Shoppe.API.Controllers.v1
{
    public class UsersController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var request = new GetUserQueryRequest { UserId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}/pictures")]
        public async Task<IActionResult> GetUserPictures(string id)
        {
            var request = new GetUserPicturesQueryRequest { UserId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}/pictures")]

        
        public async Task<IActionResult> ChangeProfilePicture(string id, [FromForm] ChangeProfilePictureCommandRequest request)
        {
            request.UserId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}/pictures/{pictureId}")]
        public async Task<IActionResult> RemoveProfilePicture(string id, string pictureId)
        {
            var request = new RemoveProfilePictureCommandRequest { UserId = id, PictureId = pictureId };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateUserCommandRequest request)
        {
            request.UserId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Deactivate(string id)
        {
            var request = new ToggleUserCommandRequest { UserId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}/roles")]
        public async Task<IActionResult> AssignRoles([FromRoute] string id, [FromBody] AssignRolesToUserCommandRequest request)
        {
            request.UserId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetRoles([FromRoute] string id)
        {
            var request = new GetUserRolesQueryRequest { UserId = id };
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviews([FromRoute] string id)
        {
            var request = new GetReviewsByUserQueryRequest { UserId = id };
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}/replies")]
        public async Task<IActionResult> GetReplies([FromRoute] string id)
        {
            var request = new GetRepliesByUserQueryRequest { UserId = id };
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteUserCommandRequest { UserId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
