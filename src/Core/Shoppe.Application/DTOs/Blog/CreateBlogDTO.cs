using Shoppe.Application.DTOs.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Blog
{
    public record CreateBlogDTO
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = [];
        public List<CreateSectionDTO> Sections { get; set; } = [];
    }
}
