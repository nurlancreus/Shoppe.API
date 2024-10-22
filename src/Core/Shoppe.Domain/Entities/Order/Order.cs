using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Entities;

namespace Shoppe.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Basket Basket { get; set; } = null!;
        public string Note { get; set; } = string.Empty;
        public ShippingAddress ShippingAddress { get; set; } = null!;
        public BillingAddress BillingAddress { get; set; } = null!;
        public Payment Payment { get; set; } = null!;
        public string ContactNumber { get; set; } = string.Empty;
        public Guid? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public string OrderCode { get; set; } = string.Empty;

        public OrderStatus OrderStatus { get; set; }
    }
}
