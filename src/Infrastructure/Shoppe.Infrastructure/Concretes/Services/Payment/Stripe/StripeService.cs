using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Payment;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Payment.Stripe
{
    public class StripeService : IStripeService
    {
        public async Task<string> CreatePaymentIntentAsync(long amount, string currency, CancellationToken cancellationToken = default)
        {
            var paymentIntentOptions = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(paymentIntentOptions, cancellationToken: cancellationToken);

            return paymentIntent.ClientSecret; // Send this client secret to the frontend.
        }


        public async Task<bool> ConfirmPaymentAsync(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId, cancellationToken: cancellationToken);

            return paymentIntent.Status == "succeeded";
        }

        public Task CancelPaymentAsync(string transactionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
