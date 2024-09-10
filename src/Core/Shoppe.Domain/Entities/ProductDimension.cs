using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class ProductDimension : BaseEntity
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public ProductDetails ProductDetails { get; set; } = null!;
    }
}
