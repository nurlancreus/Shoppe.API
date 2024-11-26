using Shoppe.Domain.Flags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Categories
{
    public class ProductCategory : Category, IDiscountable
    {
        public ICollection<Product> Products { get; set; } = [];

        [ForeignKey(nameof(Discount))]
        public Guid? DiscountId { get; set; }
        public Discount? Discount { get; set; }

    }
}
