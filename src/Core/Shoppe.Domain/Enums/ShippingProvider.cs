using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Enums
{
    public enum ShippingProvider
    {
        UPS,            // United Parcel Service
        FedEx,          // Federal Express
        DHL,            // Deutsche Post DHL
        USPS,           // United States Postal Service
        RoyalMail,      // Royal Mail (UK)
        Other           // Other or unspecified carrier
    }
}
