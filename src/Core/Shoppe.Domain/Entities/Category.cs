using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Blog> Blogs { get; set; } = [];
    }
}
