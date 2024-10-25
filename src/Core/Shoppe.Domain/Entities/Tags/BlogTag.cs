using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Tags
{
    public class BlogTag : Tag
    {
        public ICollection<Blog> Blogs { get; set; } = [];
    }
}
