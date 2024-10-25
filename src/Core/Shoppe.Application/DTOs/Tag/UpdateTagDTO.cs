using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Tag
{
    public record UpdateTagDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
