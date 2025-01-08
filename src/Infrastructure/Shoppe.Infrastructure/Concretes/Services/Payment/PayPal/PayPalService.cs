using Microsoft.Extensions.Options;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Options.Payment;

namespace Shoppe.Infrastructure.Concretes.Services.Payment.PayPal
{
    public class PayPalService : IPayPalService
    {
        private readonly PaypalClient _paypalClient;

        public PayPalService(IOptions<PaymentOptions> options, IHttpClientFactory httpClientFactory)
        {

            _paypalClient = new PaypalClient(options, httpClientFactory);
        }

        public async Task<(string paymentOrderId, string approvalUrl)> CreatePaymentAsync(double amount, string currency, string reference, CancellationToken cancellationToken = default)
        {

            var createOrderResponse = await _paypalClient.CreateOrderAsync(amount.ToString(), currency, reference, cancellationToken);

            if (createOrderResponse == null || createOrderResponse.Links == null)
            {
                throw new Exception("Error creating PayPal payment.");
            }

            var approvalUrl = createOrderResponse.Links.FirstOrDefault(link => link.Rel == "approve")?.Href ?? throw new Exception("Approval URL not found in PayPal response.");

            return (createOrderResponse.Id, approvalUrl);
        }

        public async Task<bool> CapturePaymentAsync(string paymentOrderId,  CancellationToken cancellationToken = default)
        {
            try
            {
                var captureResponse = await _paypalClient.CaptureOrderAsync(paymentOrderId, cancellationToken);

                return captureResponse?.Status == "COMPLETED";
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ConfirmPaymentAsync(string paymentOrderId, CancellationToken cancellationToken = default)
        {
            try
            {
                var orderDetails = await _paypalClient.GetOrderAsync(paymentOrderId, cancellationToken);

                // Confirm if the order status is "APPROVED" before proceeding
                if (orderDetails?.Status == "APPROVED")
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CancelPaymentAsync(string paymentReference, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(paymentReference)) throw new ArgumentNullException(nameof(paymentReference));


            var isPaymentCanceled = await _paypalClient.CancelOrderAsync(paymentReference, cancellationToken);

            if (!isPaymentCanceled)
            {
                throw new InvalidOperationException($"Failed to cancel the payment with reference: {paymentReference}");
            }

        }
    }
}
