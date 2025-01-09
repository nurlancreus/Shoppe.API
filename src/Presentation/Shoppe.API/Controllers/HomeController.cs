using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shoppe.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to the API!");
        }
    }

}
