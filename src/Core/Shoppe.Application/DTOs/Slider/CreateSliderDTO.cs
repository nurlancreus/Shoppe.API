using Shoppe.Application.DTOs.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slider
{
    public record CreateSliderDTO
    {
        public string Type { get; set; } = "Hero";
        public List<CreateSlideDTO> Slides { get; set; } = [];
    }
}
