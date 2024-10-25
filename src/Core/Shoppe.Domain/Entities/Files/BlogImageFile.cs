using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class BlogImageFile : ApplicationFile
    {
        public ICollection<Blog> Blogs { get; set; } = [];
        public ICollection<BlogBlogImage> BlogMappings { get; set; } = [];
    }
}
