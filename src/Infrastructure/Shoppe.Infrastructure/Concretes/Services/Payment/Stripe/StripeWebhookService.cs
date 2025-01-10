using Stripe;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shoppe.Application.Options.Payment;

namespace Shoppe.Application.Abstractions.Services.Payment.Stripe
{
    public class StripeWebhookService : IStripeWebhookService
    {
        private readonly string _stripeSecretKey;

        public StripeWebhookService(IOptions<PaymentOptions> options)
        {
            _stripeSecretKey = options.Value.Stripe.WebhookSecret;
        }

        public string GetSecretKey()
        {
            return _stripeSecretKey;
        }

        public bool VerifyWebhook(string payload, string? signatureHeader)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    payload,
                    signatureHeader,
                    _stripeSecretKey
                );

                return stripeEvent != null;
            }
            catch (StripeException)
            {
                return false;
            }
        }
    }
}
