using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Basket.AddBasketItem;
using Shoppe.Application.Features.Command.Basket.ClearBasket;
using Shoppe.Application.Features.Command.Basket.DeleteActiveBasket;
using Shoppe.Application.Features.Command.Basket.DeleteBasket;
using Shoppe.Application.Features.Command.Basket.DeleteBasketItem;
using Shoppe.Application.Features.Command.Basket.SyncBasket;
using Shoppe.Application.Features.Command.Basket.UpdateItemQuantity;
using Shoppe.Application.Features.Command.Coupon.Apply;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Features.Query.Basket.GetBasket;
using Shoppe.Domain.Enums;

namespace Shoppe.API.Controllers.v1
{
    public class BasketController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public BasketController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveBasket()
        {
            var request = new GetBasketQueryRequest();
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> SyncBasket([FromBody] SyncBasketCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("active")]
        public async Task<IActionResult> Delete(DeleteActiveBasketCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteBasketCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddBasketItem([FromBody] AddBasketItemCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("items")]
        public async Task<IActionResult> ClearBasket(ClearBasketCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteBasketItem(Guid id)
        {
            var request = new DeleteBasketItemCommandRequest { BasketItemId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("items/{id}")]
        public async Task<IActionResult> UpdateBasketItem([FromRoute] Guid id, [FromBody] UpdateItemQuantityCommandRequest request)
        {
            request.BasketItemId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromQuery] string couponCode)
        {
            var request = new ApplyCouponCommandRequest
            {
                CouponTarget = CouponTarget.Basket,
                CouponCode = couponCode
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
