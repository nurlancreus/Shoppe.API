using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Services.Payment.PayPal;
using Shoppe.Application.Abstractions.Services.Payment.Stripe;

namespace Shoppe.API.Controllers.v1
{
    public class WebHookController : ApplicationVersionController
    {
        private readonly IPayPalWebHookService _payPalWebhookService;
        private readonly IStripeWebHookService _stripeWebhookService;

        public WebHookController(IPayPalWebHookService payPalWebhookService, IStripeWebHookService stripeWebhookService)
        {
            _payPalWebhookService = payPalWebhookService;
            _stripeWebhookService = stripeWebhookService;
        }

        [HttpPost("paypal")]
        [AllowAnonymous]
        public async Task<IActionResult> PayPalWebHookAsync(CancellationToken cancellationToken)
        {
            string payload = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);
            await _payPalWebhookService.ReceivePayloadAsync(payload, Request.Headers, cancellationToken);

            return Ok();
        }

        [HttpPost("stripe")]
        [AllowAnonymous]
        public async Task<IActionResult> StripeWebhookAsync(CancellationToken cancellationToken)
        {
            var payload = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);
            await _stripeWebhookService.ReceivePayloadAsync(payload, Request.Headers, cancellationToken);

            return Ok();
        }
    }
}

