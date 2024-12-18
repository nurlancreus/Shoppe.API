using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Location.GetCountries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQueryRequest, GetCountriesQueryResponse>
    {
        private readonly ILocationService _locationService;

        public GetCountriesQueryHandler(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<GetCountriesQueryResponse> Handle(GetCountriesQueryRequest request, CancellationToken cancellationToken)
        {
            var countries = await _locationService.GetCountriesAsync(cancellationToken);

            return new GetCountriesQueryResponse
            {
                IsSuccess = true,
                Data = countries
            };
        }
    }
}
