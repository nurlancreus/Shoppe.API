using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slide
{
    public record GetSlideDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public GetImageFileDTO ImageFile { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
