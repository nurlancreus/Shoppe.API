using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Slider.GetAll
{
    public class GetAllSlidersQueryHandler : IRequestHandler<GetAllSlidersQueryRequest, GetAllSlidersQueryResponse>
    {
        private readonly ISliderService _sliderService;

        public GetAllSlidersQueryHandler(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<GetAllSlidersQueryResponse> Handle(GetAllSlidersQueryRequest request, CancellationToken cancellationToken)
        {
            var sliders = await _sliderService.GetAllSlidersAsync(cancellationToken);

            return new GetAllSlidersQueryResponse
            {
                IsSuccess = true,
                Data = sliders
            };
        }
    }
}
