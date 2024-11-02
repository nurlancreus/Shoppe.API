using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities.Files;
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
        public GetImageFileDTO? CoverImage { get; set; }
        public GetUserDTO? Author { get; set; }
        public List<GetSectionDTO> Sections { get; set; } = [];
        public List<GetCategoryDTO> Categories { get; set; } = [];
        public List<GetTagDTO> Tags { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
