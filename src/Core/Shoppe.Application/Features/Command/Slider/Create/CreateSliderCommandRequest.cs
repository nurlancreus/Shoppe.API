using MediatR;
using Shoppe.Application.DTOs.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Create
{
    public class CreateSliderCommandRequest : IRequest<CreateSliderCommandResponse>
    {
        public string Type { get; set; } = "Hero";
        public List<CreateSlideDTO> NewSlides { get; set; } = [];
    }
}
