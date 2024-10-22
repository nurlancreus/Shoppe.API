using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Slider.AddSlide;
using Shoppe.Application.Features.Command.Slider.Create;
using Shoppe.Application.Features.Command.Slider.Delete;
using Shoppe.Application.Features.Command.Slider.Update;
using Shoppe.Application.Features.Query.Slider.Get;

namespace Shoppe.API.Controllers.v1
{
    public class SlidersController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public SlidersController(ISender sender)
        {
            _sender = sender;
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

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromForm] UpdateSliderCommandRequest request)
        {
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
    }
}
