using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public int MaxUsage { get; set; }
        public int UsageCount { get; set; }
        public ICollection<Basket> Baskets { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
    }

}
