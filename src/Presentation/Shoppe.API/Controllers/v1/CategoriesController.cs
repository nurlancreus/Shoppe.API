using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Category.CreateCategory;
using Shoppe.Application.Features.Command.Category.DeleteCategory;
using Shoppe.Application.Features.Command.Category.UpdateCategory;
using Shoppe.Application.Features.Query.Category.GetAllCategories;
using Shoppe.Application.Features.Query.Category.GetCategoryById;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class CategoriesController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public CategoriesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesQueryRequest getAllCategoriesQueryRequest)
        {
            var response = await _sender.Send(getAllCategoriesQueryRequest);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var getCategoryByIdQueryRequest = new GetCategoryByIdQueryRequest
            {
                Id = id
            };

            var response = await _sender.Send(getCategoryByIdQueryRequest);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommandRequest createCategoryCommandRequest)
        {
            var response = await _sender.Send(createCategoryCommandRequest);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCategoryCommandRequest updateCategoryCommandRequest)
        {
            updateCategoryCommandRequest.Id = id;

            var response = await _sender.Send(updateCategoryCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var request = new DeleteCategoryCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
