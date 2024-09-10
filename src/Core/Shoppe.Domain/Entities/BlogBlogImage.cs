using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class BlogBlogImage : BaseEntity
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
        public Guid BlogImageId { get; set; }
        public BlogImageFile BlogImage { get; set; } = null!;
    }
}
