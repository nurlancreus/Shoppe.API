using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class DiscountProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid DiscountId { get; set; }
        public Product Product { get; set; } = null!;
        public Discount Discount { get; set; } = null!;
        // public bool IsActive { get; set; }
    }
}
