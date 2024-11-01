using Shoppe.Application.DTOs.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Slider
{
    public class UpdateSliderDTO
    {
        public string? SliderId { get; set; }
        public List<UpdateSlideDTO> UpdatedSlides { get; set; } = [];
        public List<CreateSlideDTO> NewSlides { get; set; } = [];

    }
}
