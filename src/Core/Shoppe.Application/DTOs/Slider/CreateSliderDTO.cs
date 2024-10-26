using Shoppe.Application.DTOs.Slide;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slider
{
    public record CreateSliderDTO
    {
        public SliderType Type { get; set; } = SliderType.Hero;
        public List<CreateSlideDTO> Slides { get; set; } = [];
    }
}
