using Shoppe.Application.DTOs.Address;
using Shoppe.Application.DTOs.Shipment;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Checkout
{
    public record CreateCheckoutDTO
    {
        public Guid BasketId { get; set; }
        public CreateBillingAddressDTO? BillingAddress { get; set; }
        public CreateShippingAddressDTO? ShippingAddress { get; set; }
        public CreateShipmentDTO? Shipment { get; set; }
        public string? CouponCode { get; set; }
        public string? Phone { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
}
