using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Info { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int Stock { get; set; }
        public Guid ProductDetailsId { get; set; }
        public ProductDetails ProductDetails { get; set; } = null!;
        public ICollection<ProductCategory> Categories { get; set; } = [];
        public ICollection<ProductImageFile> ProductImageFiles { get; set; } = [];
        public ICollection<ProductReview> Reviews { get; set; } = [];
        public ICollection<Discount> Discounts { get; set; } = [];
    }
}
