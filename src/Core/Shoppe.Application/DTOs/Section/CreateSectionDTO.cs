using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Section
{
    public record CreateSectionDTO
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TextBody { get; set; } = string.Empty;
        public FormFileCollection SectionImageFiles { get; set; } = [];
        public byte Order { get; set; }
    }
}
