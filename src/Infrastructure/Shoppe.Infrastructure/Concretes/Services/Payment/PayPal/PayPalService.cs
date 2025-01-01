using Microsoft.Extensions.Options;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Options.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Payment.PayPal
{
    public class PayPalService : IPayPalService
    {
        private readonly PaypalClient _paypalClient;
        private readonly PayPalOptions _paypalOptions;

        public PayPalService(IOptions<PaymentOptions> options, IHttpClientFactory httpClientFactory)
        {
            _paypalOptions = options.Value.PayPal;

            _paypalClient = new PaypalClient(_paypalOptions.ClientId, _paypalOptions.ClientSecret, _paypalOptions.Mode, httpClientFactory);
        }

        public async Task<string> CreatePaymentAsync(double amount, string currency, CancellationToken cancellationToken = default)
        {
            // Logic to create payment
            var reference = Guid.NewGuid().ToString();  // Unique reference for the order

            var createOrderResponse = await _paypalClient.CreateOrder(amount.ToString(), currency, reference);

            if (createOrderResponse == null || createOrderResponse.Links == null)
            {
                throw new Exception("Error creating PayPal payment.");
            }

            var approvalUrl = createOrderResponse.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;

            if (approvalUrl == null)
            {
                throw new Exception("Approval URL not found in PayPal response.");
            }

            return approvalUrl;
        }

        public async Task<bool> ExecutePaymentAsync(string paymentId, string payerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var captureResponse = await _paypalClient.CaptureOrder(paymentId);

                return captureResponse?.Status == "COMPLETED";
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
