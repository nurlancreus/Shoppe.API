using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Sections
{
    public class BlogSection : Section
    {
        [ForeignKey(nameof(Blog))]
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
        public ICollection<BlogBlogImage> BlogImageMappings { get; set; } = [];

    }
}
