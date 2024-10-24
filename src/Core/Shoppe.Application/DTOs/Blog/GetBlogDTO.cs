using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Blog
{
    public record GetBlogDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public GetUserDTO? Author { get; set; }
        public List<GetSectionDTO> Sections { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
