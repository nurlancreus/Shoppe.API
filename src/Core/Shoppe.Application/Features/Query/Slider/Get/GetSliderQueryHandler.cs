using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Slider.Get
{
    public class GetSliderQueryHandler : IRequestHandler<GetSliderQueryRequest, GetSliderQueryResponse>
    {
        private readonly ISliderService _sliderService;

        public GetSliderQueryHandler(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<GetSliderQueryResponse> Handle(GetSliderQueryRequest request, CancellationToken cancellationToken)
        {
            var slider = await _sliderService.GetSliderAsync((Guid)request.SliderId!, cancellationToken);

            return new GetSliderQueryResponse
            {
                IsSuccess = true,
                Data = slider
            };
        }
    }
}
