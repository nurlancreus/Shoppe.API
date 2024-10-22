using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.AddSlide
{
    public class AddSlideToSliderCommandRequest : IRequest<AddSlideToSliderCommandResponse>
    {
        public string? SliderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public IFormFile SlideImageFile { get; set; } = null!;
    }
}
