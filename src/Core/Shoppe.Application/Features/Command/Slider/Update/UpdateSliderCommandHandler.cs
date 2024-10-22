using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Update
{
    public class UpdateSliderCommandHandler : IRequestHandler<UpdateSliderCommandRequest, UpdateSliderCommandResponse>
    {
        public Task<UpdateSliderCommandResponse> Handle(UpdateSliderCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
