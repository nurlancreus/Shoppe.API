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
        public Guid? BlogId { get; set; }
        public string? Title { get; set; }
        public List<string> Categories { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public List<CreateSectionDTO> NewSections { get; set; } = [];
        public List<UpdateSectionDTO> UpdatedSections { get; set; } = [];
    }
}
