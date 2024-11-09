using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Files
{
    public record GetContentFileDTO : GetFileDTO
    {
        public string PreviewUrl { get; set; } = string.Empty;
    }
}
