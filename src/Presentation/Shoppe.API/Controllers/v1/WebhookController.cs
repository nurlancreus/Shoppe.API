using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shoppe.API.Controllers.v1
{
    public class WebhookController : ApplicationControllerBase
    {
        [HttpPost("paypal")]
        public async Task<IActionResult> PayPalWebhookAsync()
        {
            return Ok();
        }
    }
}
