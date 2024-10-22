using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.AddSlide
{
    public class AddSlideToSliderCommandHandler : IRequestHandler<AddSlideToSliderCommandRequest, AddSlideToSliderCommandResponse>
    {
        public Task<AddSlideToSliderCommandResponse> Handle(AddSlideToSliderCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
