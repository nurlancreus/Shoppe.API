using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Section
{
    public record UpdateSectionDTO
    {
        public string? SectionId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TextBody { get; set; }
        public FormFileCollection SectionImageFiles { get; set; } = [];
        public byte? Order { get; set; }
    }
}
