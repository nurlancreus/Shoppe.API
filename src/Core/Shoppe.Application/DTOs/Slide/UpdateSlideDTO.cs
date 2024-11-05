using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slide
{
    public record UpdateSlideDTO
    {
        public Guid? SlideId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? URL { get; set; }
        public string? ButtonText { get; set; }
       // public IFormFile? SlideImageFile { get; set; }
        public byte? Order { get; set; }
    }
}
