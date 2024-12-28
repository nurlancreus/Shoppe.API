using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shoppe.Application.DTOs.API;
using Shoppe.Application.DTOs.Location;
using Shoppe.Application.Features.Command.Coupon.Apply;
using Shoppe.Application.Features.Query.Location.GetCititesByCountry;
using Shoppe.Application.Features.Query.Location.GetCountries;
using Shoppe.Application.Options.API;
using Shoppe.Domain.Enums;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Shoppe.API.Controllers.v1
{
    public class OrdersController : ApplicationControllerBase
    {
        private readonly ISender _sender;
        private readonly APIOptions _aPICountriesOptions;
        private readonly APIOptions _aPIAmadeusOptions;
        private readonly HttpClient _httpClient;

        public OrdersController(ISender sender, IHttpClientFactory httpClientFactory, IOptionsSnapshot<APIOptions> aPIOptions)
        {
            _sender = sender;
            _httpClient = httpClientFactory.CreateClient();
            _aPICountriesOptions = aPIOptions.Get(APIOptions.CountryAPI);
            _aPIAmadeusOptions = aPIOptions.Get(APIOptions.AmadeusAPI);
        }

        [HttpGet("/countries")]
        public async Task<IActionResult> GetCountries()
        {

            var baseUrl = $"{_aPICountriesOptions.BaseUrl}v{_aPICountriesOptions.Version}";

            var url = $"{baseUrl}/name/Azerbaijan";
            var response = await _httpClient.GetAsync($"{url}/?fields=borders");

            if (!response.IsSuccessStatusCode)
                return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize JSON dynamically using JsonDocument
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);

            var root = doc.RootElement;

            // Access borders and exclude a specific country
            var neighbors = root
                .EnumerateArray()
                .SelectMany(item => item.GetProperty("borders").EnumerateArray())
                .Select(border => border.GetString())
                .Where(border => border != "ARM")
                .ToList();

            List<GetCountryDTO> countries = [];

            foreach (var code in neighbors)
            {
                var urll = $"{baseUrl}/alpha/{code}/?fields=name,cca2,flags";

                var responsee = await _httpClient.GetAsync(urll);
                if (!responsee.IsSuccessStatusCode)
                    continue;

                var jsonResponsee = await responsee.Content.ReadAsStringAsync();

                var json = JsonSerializer.Deserialize<GetCountryDTO>(jsonResponsee);

                if (json != null)
                    countries.Add(json);
            }

            return Ok(countries);

        }

        [HttpGet("/countries/{countryCode}/citites")]
        public async Task<IActionResult> GetCitiesByCountry([FromRoute] string countryCode)
        {
            var baseUrl = $"{_aPIAmadeusOptions.BaseUrl}v{_aPIAmadeusOptions.Version}";
            var tokenUrl = $"{baseUrl}/security/oauth2/token";

            // Form data
            var content = new StringContent(
                $"grant_type=client_credentials&client_id={_aPIAmadeusOptions.ApiKey}&client_secret={_aPIAmadeusOptions.ApiSecret}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            // Send POST request
            var postResponse = await _httpClient.PostAsync(tokenUrl, content);

            if (!postResponse.IsSuccessStatusCode)
            {
                var error = await postResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve access token. Error: {error}");
            }

            // Parse JSON response
            var tokenJsonResponse = await postResponse.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<AmadeusAPITokenDTO>(tokenJsonResponse);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new Exception("Invalid token response.");
            }

            // Set the Bearer token in the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var url = $"{baseUrl}/reference-data/locations/cities";

            // Send GET request with the token
            var response = await _httpClient.GetAsync($"https://backstage.taboola.com/backstage/api/1.0/resources/countries/us/cities");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
                throw new Exception($"Failed to retrieve cities. Error: {error}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return Ok(jsonResponse);
        }

        [HttpPatch("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromQuery] string couponCode)
        {
            var request = new ApplyCouponCommandRequest
            {
                CouponTarget = CouponTarget.Order,
                CouponCode = couponCode
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

    }
}
