using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Files
{
    public record GetImageFileDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string PathName { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
