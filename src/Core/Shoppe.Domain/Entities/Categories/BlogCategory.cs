using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Categories
{
    public class BlogCategory : Category
    {
        public ICollection<Blog> Blogs { get; set; } = [];

    }
}
