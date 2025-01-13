using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shoppe.API.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ShipmentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _httpClient.GetAsync("https://localhost:7004/shipping");

            var content = await response.Content.ReadAsStringAsync();

            return Ok(content);
        }
    }
}
