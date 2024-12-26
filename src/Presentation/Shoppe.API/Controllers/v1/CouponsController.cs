using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Coupon.Create;
using Shoppe.Application.Features.Command.Coupon.Delete;
using Shoppe.Application.Features.Command.Coupon.Toggle;
using Shoppe.Application.Features.Command.Coupon.Update;
using Shoppe.Application.Features.Command.Discount.Toggle;
using Shoppe.Application.Features.Query.Contact.GetContactById;
using Shoppe.Application.Features.Query.Coupon.GetAll;

namespace Shoppe.API.Controllers.v1
{
    public class CouponsController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public CouponsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCouponsQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var request = new GetContactByIdQueryRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCouponCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateCouponCommandRequest request)
        {
            request.Id = id;
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteCouponCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var request = new ToggleCouponCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
