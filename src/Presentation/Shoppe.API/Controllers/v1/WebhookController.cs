using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Services.Payment.PayPal;
using Shoppe.Application.Abstractions.Services.Payment.Stripe;
using Shoppe.Application.DTOs.Payment;
using Shoppe.Domain.Constants.EventTypes;
using Shoppe.Domain.Events;
using Stripe;

namespace Shoppe.API.Controllers.v1
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly IPayPalWebhookService _payPalWebhookService;
        private readonly IStripeWebhookService _stripeWebhookService;
        private readonly IMediator _mediator;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(IPayPalWebhookService payPalWebhookService,
                                 IStripeWebhookService stripeWebhookService,
                                 IMediator mediator,
                                 ILogger<WebhookController> logger)
        {
            _payPalWebhookService = payPalWebhookService;
            _stripeWebhookService = stripeWebhookService;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("paypal")]
        [AllowAnonymous]
        public async Task<IActionResult> PayPalWebhookAsync(CancellationToken cancellationToken)
        {
            var rawBody = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);

            if (!await _payPalWebhookService.VerifyWebhookAsync(rawBody, Request.Headers, cancellationToken))
            {
                _logger.LogWarning("Invalid PayPal webhook signature.");
                return Unauthorized();
            }

            var notification = System.Text.Json.JsonSerializer.Deserialize<PayPalWebhookNotification>(rawBody);

            if (notification?.EventType == PayPalWebhookEventTypes.PAYMENT_CAPTURE_COMPLETED)
            {
                var paymentOrderId = notification.Resource.ParentPayment;
                await _mediator.Publish(new PaymentCaptureCompletedEvent(paymentOrderId), cancellationToken);
                return Ok();
            }

            if (notification?.EventType == PayPalWebhookEventTypes.PAYMENT_CAPTURE_DECLINED)
            {
                var paymentOrderId = notification.Resource.ParentPayment;
                await _mediator.Publish(new PaymentCaptureDeclinedEvent(paymentOrderId), cancellationToken);
                return Ok();
            }

            if (notification?.EventType == PayPalWebhookEventTypes.PAYMENT_ORDER_CANCELLED)
            {
                var paymentOrderId = notification.Resource.ParentPayment;
                await _mediator.Publish(new PaymentOrderCancelledEvent(paymentOrderId), cancellationToken);
                return Ok();
            }

            if (notification?.EventType == PayPalWebhookEventTypes.PAYMENT_REFUND_COMPLETED)
            {
                var paymentOrderId = notification.Resource.ParentPayment;
                await _mediator.Publish(new PaymentRefundCompletedEvent(paymentOrderId), cancellationToken);
                return Ok();
            }

            return BadRequest("Unsupported event type.");
        }

        [HttpPost("stripe")]
        [AllowAnonymous]
        public async Task<IActionResult> StripeWebhookAsync(CancellationToken cancellationToken)
        {
            var payload = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);
            var stripeSignature = Request.Headers["Stripe-Signature"];

            if (!_stripeWebhookService.VerifyWebhook(payload, stripeSignature))
            {
                _logger.LogWarning("Invalid Stripe webhook signature.");
                return Unauthorized();
            }

            var stripeEvent = EventUtility.ConstructEvent(payload, stripeSignature, _stripeWebhookService.GetSecretKey());

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
                    {

                        await _mediator.Publish(new PaymentCaptureCompletedEvent(paymentIntent.Id), cancellationToken);
                    }
                    break;

                case EventTypes.ChargeCaptured:
                    if (stripeEvent.Data.Object is Charge charge)
                    {
                        // Capture charge logic
                        var chargeCapturedEvent = new PaymentCaptureCompletedEvent(charge.Id);
                        await _mediator.Publish(chargeCapturedEvent, cancellationToken);
                    }
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    if (stripeEvent.Data.Object is PaymentIntent failedPaymentIntent)
                    {
                        //await _mediator.Publish(new PaymentCaptureDeclinedEvent(failedPaymentIntent.Id), cancellationToken);
                    }
                    break;

                case EventTypes.InvoicePaymentFailed:
                    if (stripeEvent.Data.Object is Invoice invoice)
                    {
                        //await _mediator.Publish(new PaymentCaptureDeclinedEvent(invoice.Id), cancellationToken);
                    }
                    break;

                case EventTypes.ChargeRefunded:
                    if (stripeEvent.Data.Object is PaymentIntent refundedPaymentIntent)
                    {
                        //await _mediator.Publish(new PaymentRefundCompletedEvent(refundedPaymentIntent.Id), cancellationToken);
                    }
                    break;

                default:
                    _logger.LogWarning("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                    return BadRequest("Unsupported event type.");
            }

            return Ok();
        }
    }
}

