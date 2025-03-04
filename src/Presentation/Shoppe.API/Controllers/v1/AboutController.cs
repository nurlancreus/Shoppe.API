﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using Shoppe.Application.Features.Command.About.Update;
using Shoppe.Application.Features.Query.About.Get; 
using Shoppe.Application.Features.Command.About.RemoveImage;
using Microsoft.AspNetCore.Authorization;

namespace Shoppe.API.Controllers.v1
{
    [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
    public class AboutController : ApplicationVersionController
    {
        private readonly ISender _sender;

        public AboutController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult<GetAboutQueryResponse>> GetAbout()
        {
            var request = new GetAboutQueryRequest();
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<ActionResult<UpdateAboutCommandResponse>> UpdateAbout([FromForm] UpdateAboutCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> RemoveImage(Guid imageId)
        {
            var request = new RemoveImageCommandRequest
            {
                ImageId = imageId
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
