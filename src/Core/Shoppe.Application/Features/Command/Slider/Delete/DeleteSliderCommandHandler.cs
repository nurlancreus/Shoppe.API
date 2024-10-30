using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Delete
{
    public class DeleteSliderCommandHandler : IRequestHandler<DeleteSliderCommandRequest, DeleteSliderCommandResponse>
    {
        private readonly ISliderService _sliderService;

        public DeleteSliderCommandHandler(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<DeleteSliderCommandResponse> Handle(DeleteSliderCommandRequest request, CancellationToken cancellationToken)
        {
            await _sliderService.DeleteSliderAsync(request.SliderId!, cancellationToken);

            return new DeleteSliderCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
