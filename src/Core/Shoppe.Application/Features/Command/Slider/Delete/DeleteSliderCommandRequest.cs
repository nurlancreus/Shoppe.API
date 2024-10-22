using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Delete
{
    public class DeleteSliderCommandRequest : IRequest<DeleteSliderCommandResponse>
    {
        public string? SliderId { get; set; }
    }
}
