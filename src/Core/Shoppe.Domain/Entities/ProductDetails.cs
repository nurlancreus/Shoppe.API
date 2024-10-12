using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class ProductDetails : BaseEntity
    {
        public float Weigth { get; set; }
        public ICollection<Material> Materials { get; set; } = [];
        public ICollection<Color> Colors { get; set; } = [];
        public ProductDimension Dimension { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
