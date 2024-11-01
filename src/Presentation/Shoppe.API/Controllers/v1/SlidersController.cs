using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Slider.AddSlide;
using Shoppe.Application.Features.Command.Slider.Create;
using Shoppe.Application.Features.Command.Slider.Delete;
using Shoppe.Application.Features.Command.Slider.RemoveImage;
using Shoppe.Application.Features.Command.Slider.Update;
using Shoppe.Application.Features.Query.Slider.Get;
using Shoppe.Application.Features.Query.Slider.GetAll;
using Shoppe.Application.Features.Query.Tag.GetAll;

namespace Shoppe.API.Controllers.v1
{
    public class SlidersController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public SlidersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var request = new GetAllSlidersQueryRequest();
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpGet("{sliderId}")]
        public async Task<IActionResult> DeleteSlider(string sliderId)
        {
            var request = new GetSliderQueryRequest { SliderId = sliderId };
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateSliderCommandRequest request)
        {
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPost("{sliderId}/add-slide")]
        public async Task<IActionResult> AddSlideToSlider(string sliderId, [FromForm] AddSlideToSliderCommandRequest request)
        {
            request.SliderId = sliderId;
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateSliderCommandRequest request)
        {
            request.SliderId = id;
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpDelete("{sliderId}")]
        public async Task<IActionResult> Delete(string sliderId)
        {
            var request = new DeleteSliderCommandRequest { SliderId = sliderId };
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPatch("{slideId}/images")]
        public async Task<IActionResult> ChangeImage(string slideId, [FromForm] ChangeSlideImageCommandRequest request)
        {
            request.SlideId = slideId;

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
