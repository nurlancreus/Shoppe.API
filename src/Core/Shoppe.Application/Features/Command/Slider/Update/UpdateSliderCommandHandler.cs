using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Slider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Update
{
    public class UpdateSliderCommandHandler : IRequestHandler<UpdateSliderCommandRequest, UpdateSliderCommandResponse>
    {
        private readonly ISliderService _sliderService;

        public UpdateSliderCommandHandler(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<UpdateSliderCommandResponse> Handle(UpdateSliderCommandRequest request, CancellationToken cancellationToken)
        {
            await _sliderService.UpdateSliderAsync(new UpdateSliderDTO
            {
                SliderId = request.SliderId,
                Slides = request.Slides
            }, cancellationToken);

            return new UpdateSliderCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
