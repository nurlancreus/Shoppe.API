using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class BlogBlogImage
    {

        [ForeignKey(nameof(BlogSection))]
        public Guid BlogSectionId { get; set; }
        public BlogSection BlogSection { get; set; } = null!;
        public Guid BlogImageId { get; set; }
        public BlogImageFile BlogImage { get; set; } = null!;
    }
}
