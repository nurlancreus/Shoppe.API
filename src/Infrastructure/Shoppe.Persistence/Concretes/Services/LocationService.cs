using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.LocationRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class LocationService : ILocationService
    {
        private readonly ICountryReadRepository _countryReadRepository;
        private readonly ICityReadRepository _cityReadRepository;
        private readonly IShippingCalculatorService _shippingCalculatorService;

        public LocationService(ICountryReadRepository countryReadRepository, ICityReadRepository cityReadRepository, IShippingCalculatorService shippingCalculatorService)
        {
            _countryReadRepository = countryReadRepository;
            _cityReadRepository = cityReadRepository;
            _shippingCalculatorService = shippingCalculatorService;
        }

        public async Task<List<GetCityDTO>> GetCitiesByCountryAsync(Guid countryId, CancellationToken cancellationToken = default)
        {
            var citites = await _cityReadRepository.Table.Where(c => c.CountryId == countryId).ToListAsync(cancellationToken);

            return citites.Select(c => new GetCityDTO { Id = c.Id, Name = c.Name, ShippingCost = _shippingCalculatorService.CalculateShippingCost(distance: 100m) }).ToList();
        }

        public async Task<List<GetCountryDTO>> GetCountriesAsync(CancellationToken cancellationToken = default)
        {
          

            return [];
        }
    }
}
