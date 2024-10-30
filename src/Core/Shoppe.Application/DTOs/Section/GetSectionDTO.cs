using Shoppe.Application.DTOs.Files;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Section
{
    public record GetSectionDTO
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? TextBody { get; set; }
        public ICollection<GetImageFileDTO> ImageFiles { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public byte Order { get; set; }
    }
}
