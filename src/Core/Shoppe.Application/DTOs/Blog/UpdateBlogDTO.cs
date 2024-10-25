using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Blog
{
    public record UpdateBlogDTO
    {
        public string? BlogId { get; set; }
        public string? Title { get; set; }
        public List<string> Categories { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public FormFile? CoverImageFile { get; set; }
        public List<CreateSectionDTO> Sections { get; set; } = [];
        public List<UpdateSectionDTO> UpdatedSections { get; set; } = [];
    }
}
