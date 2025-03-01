using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Slider.AddSlide;
using Shoppe.Application.Features.Command.Slider.Create;
using Shoppe.Application.Features.Command.Slider.Delete;
using Shoppe.Application.Features.Command.Slider.RemoveImage;
using Shoppe.Application.Features.Command.Slider.Update;
using Shoppe.Application.Features.Query.Slider.Get;
using Shoppe.Application.Features.Query.Slider.GetAll;

namespace Shoppe.API.Controllers.v1
{
    [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
    public class SlidersController : ApplicationVersionController
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
        public async Task<IActionResult> DeleteSlider(Guid sliderId)
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
        public async Task<IActionResult> AddSlideToSlider(Guid sliderId, [FromForm] AddSlideToSliderCommandRequest request)
        {
            request.SliderId = sliderId;
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateSliderCommandRequest request)
        {
            request.SliderId = id;
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpDelete("{sliderId}")]
        public async Task<IActionResult> Delete(Guid sliderId)
        {
            var request = new DeleteSliderCommandRequest { SliderId = sliderId };
            var response = await _sender.Send(request);
            return Ok(response);
        }

        [HttpPatch("{slideId}/images")]
        public async Task<IActionResult> ChangeImage(Guid slideId, [FromForm] ChangeSlideImageCommandRequest request)
        {
            request.SlideId = slideId;

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
