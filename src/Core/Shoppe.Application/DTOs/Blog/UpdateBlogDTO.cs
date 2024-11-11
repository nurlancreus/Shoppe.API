using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Content;
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
        public string? Content { get; set; }
        public ICollection<CreateContentImageFileDTO> ContentImages { get; set; } = [];
    }
}
