using Stripe;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shoppe.Application.Options.Payment;
using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Shoppe.Domain.Exceptions;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Abstractions.Services.Payment.Stripe;

namespace Shoppe.Infrastructure.Concretes.Services.Payment.Stripe
{
    public class StripeWebHookService : IStripeWebHookService
    {
        private readonly string _stripeSecretKey;
        private readonly ILogger<StripeWebHookService> _logger;
        private readonly IPaymentEventService _paymentEventService;
        public StripeWebHookService(IOptions<PaymentOptions> options, ILogger<StripeWebHookService> logger, IPaymentEventService paymentEventService)
        {
            _stripeSecretKey = options.Value.Stripe.WebhookSecret;
            _logger = logger;
            _paymentEventService = paymentEventService;
        }

        public async Task ReceivePayloadAsync(string payload, IHeaderDictionary headers, CancellationToken cancellationToken = default)
        {
            var stripeSignature = headers["Stripe-Signature"];

            Event stripeEvent = GetStripeEvent(payload, stripeSignature);

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
                        await _paymentEventService.PaymentSucceededAsync(paymentIntent.Id, cancellationToken);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    if (stripeEvent.Data.Object is PaymentIntent failedPaymentIntent)
                        await _paymentEventService.PaymentFailedAsync(failedPaymentIntent.Id, cancellationToken);
                    break;

                case EventTypes.PaymentIntentCanceled:
                    if (stripeEvent.Data.Object is PaymentIntent canceledPaymentIntent)
                        await _paymentEventService.PaymentCanceledAsync(canceledPaymentIntent.Id, cancellationToken);
                    break;

                case EventTypes.ChargeRefunded:
                    if (stripeEvent.Data.Object is Charge refundedCharge)
                        await _paymentEventService.PaymentRefundedAsync(refundedCharge.PaymentIntentId, cancellationToken);
                    break;


                default:
                    _logger.LogWarning("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                    throw new PaymentWebHookException($"Unsupported Stripe event type: {stripeEvent.Type}");
            }

        }

        private Event GetStripeEvent(string payload, string? stripeSignature)
        {
            try
            {
                return EventUtility.ConstructEvent(payload, stripeSignature, _stripeSecretKey);
            }
            catch (StripeException ex)
            {
                _logger.LogWarning("Invalid Stripe webhook signature: {Message}", ex.Message);
                throw new PaymentWebHookException("Invalid Stripe webhook signature.");
            }
        }
    }
}
