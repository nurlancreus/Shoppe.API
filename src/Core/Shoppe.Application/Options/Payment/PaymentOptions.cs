using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.Payment
{
    public class PaymentOptions
    {
        public StripeOptions Stripe { get; set; } = null!;
        public PayPalOptions PayPal { get; set; } = null!;
    }
}
