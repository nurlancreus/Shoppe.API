using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Delete
{
    public class DeleteSliderCommandHandler : IRequestHandler<DeleteSliderCommandRequest, DeleteSliderCommandResponse>
    {
        public Task<DeleteSliderCommandResponse> Handle(DeleteSliderCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
