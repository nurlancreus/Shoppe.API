using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Files
{
    public record GetFileDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string PathName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
