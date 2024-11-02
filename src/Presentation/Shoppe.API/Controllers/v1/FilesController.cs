using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Query.Files.GetAllImageFiles;

namespace Shoppe.API.Controllers.v1
{

    public class FilesController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public FilesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("/images")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllImageFIlesQueryRequest request)
        {
            request.Type = null;

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
