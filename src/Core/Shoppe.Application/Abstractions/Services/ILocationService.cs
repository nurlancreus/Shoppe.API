using Shoppe.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface ILocationService
    {
        Task<List<GetCountryDTO>> GetCountriesAsync(CancellationToken cancellationToken = default);
        Task<List<GetCityDTO>> GetCitiesByCountryAsync(Guid countryId, CancellationToken cancellationToken = default);
    }
}
