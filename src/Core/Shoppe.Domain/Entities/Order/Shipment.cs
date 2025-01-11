using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Shipment : BaseEntity
    {
        public Guid OrderId { get; set; }     // Foreign key to the associated Order
        public Order Order { get; set; } = null!; // Navigation property to the related Order

        public string TrackingNumber { get; set; } = string.Empty; // Tracking number provided by the shipping provider
        public ShippingProvider Provider { get; set; } // Shipping provider (UPS, FedEx, etc.)
        public DateTime EstimatedDeliveryDate { get; set; } // Estimated delivery date

        public ShippingStatus Status { get; set; }  // Current shipment status (Shipped, InTransit, etc.)
        public DeliveryMethod Method { get; set; } // Delivery method (Standard, Express, etc.)
        public double Cost { get; set; }        // The cost of shipping
        public bool IsShipped { get; set; }  // Whether the order has been shipped (bool for easier checks)
    }

}
