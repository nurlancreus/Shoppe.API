using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Content;
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
        public List<string> Tags { get; set; } = [];
        public IFormFile CoverImageFile { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public ICollection<CreateContentImageFileDTO> ContentImages { get; set; } = []; 
    }
}
