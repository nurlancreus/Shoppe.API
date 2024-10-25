using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Tag
{
    public record CreateTagDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TagType Type { get; set; } = TagType.Blog;
    }
}
