using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class BlogBlogImage
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
        public Guid BlogImageId { get; set; }
        public BlogImageFile BlogImage { get; set; } = null!;
        public bool IsMain { get; set; }
    }
}
