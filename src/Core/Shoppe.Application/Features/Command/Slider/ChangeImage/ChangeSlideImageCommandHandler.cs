using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Slider.RemoveImage
{
    public class ChangeSlideImageCommandHandler : IRequestHandler<ChangeSlideImageCommandRequest, ChangeSlideImageCommandResponse>
    {
        private readonly ISliderService _sliderService;

        public ChangeSlideImageCommandHandler(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<ChangeSlideImageCommandResponse> Handle(ChangeSlideImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _sliderService.ChangeSlideImageAsync((Guid)request.SlideId!, request.NewImageFile, cancellationToken);

            return new ChangeSlideImageCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
