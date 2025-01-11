using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Enums
{
    public enum ShippingStatus
    {
        Pending,        // Shipment is waiting to be processed
        Shipped,        // Shipment has been dispatched
        InTransit,      // Shipment is in transit
        Delivered,      // Shipment has been delivered to the recipient
        Failed,         // Shipment failed or encountered an issue
        Canceled        // Shipment was canceled
    }
}
