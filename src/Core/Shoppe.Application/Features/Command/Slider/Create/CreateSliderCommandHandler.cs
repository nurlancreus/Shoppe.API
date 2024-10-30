using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Slider;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.Create
{
    public class CreateSliderCommandHandler : IRequestHandler<CreateSliderCommandRequest, CreateSliderCommandResponse>
    {
        private readonly ISliderService _sliderService;
        public CreateSliderCommandHandler(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<CreateSliderCommandResponse> Handle(CreateSliderCommandRequest request, CancellationToken cancellationToken)
        {
            var slider = new CreateSliderDTO
            {
                Slides = request.Slides,
            };

            if (Enum.TryParse(request.Type, true, out SliderType sliderType))
            {
                slider.Type = sliderType;
            }

            await _sliderService.CreateSliderAsync(slider, cancellationToken);

            return new CreateSliderCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
