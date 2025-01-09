using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.DTOs.Payment;
using Shoppe.Domain.Events;
using System.Threading;

namespace Shoppe.API.Controllers.v1
{
    public class WebhookController : ApplicationControllerBase
    {
        private readonly IMediator _mediator;

        public WebhookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("paypal")]
        public async Task<IActionResult> PayPalWebhookAsync([FromBody] PayPalWebhookNotification notification, CancellationToken cancellationToken)
        {
            if (notification.EventType == "PAYMENT.CAPTURE.COMPLETED")
            {
                // Extract the necessary details for PAYMENT.CAPTURE.COMPLETED
                var paymentOrderId = notification.Resource.ParentPayment;
                var paymentCompletedEvent = new PaymentCaptureCompletedEvent(paymentOrderId);

                await _mediator.Publish(paymentCompletedEvent, cancellationToken);

                return Ok();
            }

            return BadRequest("Unsupported event type.");
        }
    }
}
