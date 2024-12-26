using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Address.Billing.Create;
using Shoppe.Application.Features.Command.Address.Billing.Update;
using Shoppe.Application.Features.Command.Address.Common.Clear;
using Shoppe.Application.Features.Command.Address.Common.Delete;
using Shoppe.Application.Features.Command.Address.Shipping.Create;
using Shoppe.Application.Features.Command.Address.Shipping.Update;
using Shoppe.Application.Features.Query.Address.Shipping.Get;

namespace Shoppe.API.Controllers.v1
{
    public class AddressesController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public AddressesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("shipping")]
        public async Task<IActionResult> GetShipping()
        {
            var request = new GetShippingAddressQueryRequest();

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("billing")]
        public async Task<IActionResult> GetBilling()
        {
            var request = new GetShippingAddressQueryRequest();

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("billing")]
        public async Task<IActionResult> CreateBilling(CreateBillingAddressCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("shipping")]
        public async Task<IActionResult> CreateShipping(CreateShippingAddressCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("billing")]
        public async Task<IActionResult> UpdateBilling(UpdateBillingAddressCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("shipping")]
        public async Task<IActionResult> UpdateShipping(UpdateShippingAddressCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteAddressCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            var request = new ClearAddressCommandRequest();

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
