using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class ProductCategory : Category
    {
        public ICollection<Product> Products { get; set; } = [];

    }
}
