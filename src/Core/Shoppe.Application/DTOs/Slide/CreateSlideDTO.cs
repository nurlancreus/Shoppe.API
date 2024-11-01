using Microsoft.AspNetCore.Http;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slide
{
    public record CreateSlideDTO
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public IFormFile SlideImageFile { get; set; } = null!;
        public byte Order { get; set; }

    }
}
