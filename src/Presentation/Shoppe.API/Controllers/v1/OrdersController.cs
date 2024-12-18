using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Query.Location.GetCititesByCountry;
using Shoppe.Application.Features.Query.Location.GetCountries;

namespace Shoppe.API.Controllers.v1
{
    public class OrdersController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public OrdersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("/countries")]
        public async Task<IActionResult> GetCountries()
        {
            var request = new GetCountriesQueryRequest();

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("/countries/{countryId}/citites")]
        public async Task<IActionResult> GetCitiesByCountry([FromRoute] Guid countryId)
        {
            var request = new GetCitiesByCountryQueryRequest { CountryId = countryId };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
