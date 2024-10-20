using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Basket : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public ICollection<BasketItem> Items { get; set; } = [];
        public Order? Order { get; set; } = null!;
        public Guid? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
    }
}
