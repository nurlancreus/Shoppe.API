using Shoppe.Application.Abstractions.Services.Payment;
using Stripe;


namespace Shoppe.Infrastructure.Concretes.Services.Payment.Stripe
{
    public class StripeService : IStripeService
    {
        public async Task<(string paymentIntentId, string clientSecret)> CreatePaymentIntentAsync(long amount, string currency, CancellationToken cancellationToken = default)
        {
            var paymentIntentOptions = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = ["card"],
                CaptureMethod = "manual", // Use manual capture method
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(paymentIntentOptions, cancellationToken: cancellationToken);

            return (paymentIntent.Id, paymentIntent.ClientSecret);
        }

        public async Task<bool> ConfirmPaymentAsync(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId, cancellationToken: cancellationToken);

                return paymentIntent.Status == "succeeded";
            }
            catch (StripeException)
            {

                return false;
            }
        }

        public async Task<bool> CapturePaymentAsync(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId, cancellationToken: cancellationToken);

                // Only capture if the payment is authorized (requires capture)
                if (paymentIntent.Status == "requires_capture")
                {
                    var captureOptions = new PaymentIntentCaptureOptions
                    {
                        AmountToCapture = paymentIntent.Amount,
                    };

                    await service.CaptureAsync(paymentIntentId, captureOptions, cancellationToken: cancellationToken);
                    return true;
                }

                // If payment is already captured or succeeded, return false
                return false;
            }
            catch (StripeException)
            {
                return false;
            }
        }

        public async Task<bool> CancelPaymentAsync(string paymentIntentId, CancellationToken cancellationToken)
        {
            try
            {
                var service = new PaymentIntentService();

                var paymentIntent = await service.GetAsync(paymentIntentId, cancellationToken: cancellationToken);

                if (paymentIntent.Status == "requires_payment_method" || paymentIntent.Status == "requires_capture" || paymentIntent.Status == "requires_confirmation" || paymentIntent.Status == "requires_action" || paymentIntent.Status == "processing")
                {
                    await service.CancelAsync(paymentIntentId, cancellationToken: cancellationToken);
                    return true;
                }

                if (paymentIntent.Status == "succeeded")
                {
                    var refundService = new RefundService();
                    await refundService.CreateAsync(new RefundCreateOptions
                    {
                        PaymentIntent = paymentIntentId
                    }, cancellationToken: cancellationToken);
                    return true;
                }

                return false;
            }
            catch (StripeException)
            {

                return false;
            }
        }
    }
}
