using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class DiscountCategory
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DiscountId { get; set; }
        public ProductCategory Category { get; set; } = null!;
        public Discount Discount { get; set; } = null!;
        // public bool IsActive { get; set; }

    }
}
