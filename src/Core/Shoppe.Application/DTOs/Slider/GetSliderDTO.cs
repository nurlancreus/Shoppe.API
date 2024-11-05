using Shoppe.Application.DTOs.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slider
{
    public record GetSliderDTO
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<GetSlideDTO> Slides { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public byte Order { get; set; }

    }
}
