using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment.Stripe
{
    public interface IStripeWebhookService
    {
        bool VerifyWebhook(string payload, string? signatureHeader);
        string GetSecretKey();
    }
}
