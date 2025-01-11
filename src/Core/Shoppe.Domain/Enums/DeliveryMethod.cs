using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Enums
{
    public enum DeliveryMethod
    {
        Standard,       // Standard delivery, usually 3-7 business days
        Express,        // Expedited shipping, typically 1-3 business days
        NextDay,        // Next-day delivery
        TwoDay,         // Two-day delivery
        SameDay,        // Same-day delivery
        Pickup          // Pickup option (customer picks up from a local store/warehouse)
    }
}
