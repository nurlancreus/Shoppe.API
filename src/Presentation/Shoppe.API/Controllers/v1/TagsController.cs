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
        public async Task<IActionResult> GetAll([FromQuery] GetAllTagsQueryRequest getAllTagsQueryRequest)
        {
            var response = await _sender.Send(getAllTagsQueryRequest);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var getTagByIdQueryRequest = new GetTagByIdQueryRequest
            {
                Id = id
            };

            var response = await _sender.Send(getTagByIdQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagCommandRequest createTagCommandRequest)
        {
            var response = await _sender.Send(createTagCommandRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateTagCommandRequest updateTagCommandRequest)
        {
            updateTagCommandRequest.Id = id;

            var response = await _sender.Send(updateTagCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var request = new DeleteTagCommandRequest { Id = id };

            var response = await _sender.Send(request);
            return Ok(response);
        }
    }
}
