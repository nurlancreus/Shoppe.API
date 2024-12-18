using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Location.GetCititesByCountry
{
    public class GetCitiesByCountryQueryHandler : IRequestHandler<GetCitiesByCountryQueryRequest, GetCitiesByCountryQueryResponse>
    {
        private readonly ILocationService _locationService;

        public GetCitiesByCountryQueryHandler(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<GetCitiesByCountryQueryResponse> Handle(GetCitiesByCountryQueryRequest request, CancellationToken cancellationToken)
        {
            var cities = await _locationService.GetCitiesByCountryAsync((Guid)request.CountryId!, cancellationToken);

            return new GetCitiesByCountryQueryResponse
            {
                IsSuccess = true,
                Data = cities
            };
        }
    }
}
