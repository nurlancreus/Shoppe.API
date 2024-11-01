using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.RemoveImage
{
    public class ChangeSlideImageCommandRequest : IRequest<ChangeSlideImageCommandResponse>
    {
        public string? SlideId { get; set; }
        public required IFormFile NewImageFile { get; set; }
    }
}
