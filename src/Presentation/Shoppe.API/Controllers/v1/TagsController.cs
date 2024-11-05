using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Tag.Create;
using Shoppe.Application.Features.Command.Tag.Delete;
using Shoppe.Application.Features.Command.Tag.Update;
using Shoppe.Application.Features.Query.Tag.Get;
using Shoppe.Application.Features.Query.Tag.GetAll;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class TagsController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public TagsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllTagsQueryRequest request)
        {
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetTagByIdQueryRequest
            {
                Id = id
            };

            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagCommandRequest request)
        {
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTagCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteTagCommandRequest { Id = id };

            var response = await _sender.Send(request);
            return Ok(response);
        }
    }
}
