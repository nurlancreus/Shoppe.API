using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class ProductDetails
    {
        public float Weight { get; set; }
        public ICollection<Material> Materials { get; set; } = [];
        public ICollection<Color> Colors { get; set; } = [];
        public float Height { get; set; }
        public float Width { get; set; }
    }
}
