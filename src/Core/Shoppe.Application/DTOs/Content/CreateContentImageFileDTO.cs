using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Content
{
    public record CreateContentImageFileDTO
    {
        public string PreviewUrl { get; set; } = string.Empty;
        public IFormFile ImageFile { get; set; } = null!;
    }
}
