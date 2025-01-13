using Microsoft.Extensions.Options;
using Mock.ShippingProvider.Application.DTOs;
using Mock.ShippingProvider.Application.Interfaces;
using Mock.ShippingProvider.Application.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mock.ShippingProvider.Infrastructure.Services
{
    public class GeoInfoService : IGeoInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly APIOptions _apiOptions;

        public GeoInfoService(IHttpClientFactory httpClientFactory, IOptionsSnapshot<APIOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiOptions = options.Get(APIOptions.GeoCodeAPI);
        }

        public async Task<CountryGeoInfo> GetCountryGeoInfoByNameAsync(string countryName)
        {
            // Define the API endpoint
            var url = $"{_apiOptions.BaseUrl}/search?q={countryName}&format=json&limit=1&api_key={_apiOptions.ApiKey}";

            // Send the GET request
            var response = await _httpClient.GetStringAsync(url);

            // Deserialize the response JSON into a list of objects
            var geoInfoList = JsonSerializer.Deserialize<List<GeoInfo>>(response);

            // Return the first result (or null if no results)
            if (geoInfoList == null || geoInfoList.Count == 0)
            {
                // implement results pattern 
                return null;
            }

            var geoInfo = geoInfoList[0];

            // Map the response to the CountryGeoInfo DTO
            return new CountryGeoInfo
            {
                Country = geoInfo.DisplayName,
                Latitude = double.Parse(geoInfo.Lat),
                Longitude = double.Parse(geoInfo.Lon),
            };
        }
    }

    // Helper model to deserialize the API response
    public class GeoInfo
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;
        [JsonPropertyName("lat")]
        public string Lat { get; set; } = string.Empty;
        [JsonPropertyName("lon")]
        public string Lon { get; set; } = string.Empty;
    }
}
