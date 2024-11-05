using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Shoppe.Application.Features.Command.About.Update;
using Shoppe.Application.Features.Query.About.Get; // Assume there's a GetAboutQuery
using System.Threading.Tasks;
using Shoppe.Application.Features.Command.About.RemoveImage;

namespace Shoppe.API.Controllers.v1
{
    public class AboutController : ApplicationControllerBase
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

        [HttpDelete("{sectionId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(Guid sectionId, Guid imageId)
        {
            var request = new RemoveImageCommandRequest
            {
                SectionId = sectionId,
                ImageId = imageId
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
