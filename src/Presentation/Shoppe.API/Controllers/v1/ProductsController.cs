using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Domain.Entities;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class ProductsController : ApplicationControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommandRequest createProductCommandRequest)
        {
            var response = await _mediator.Send(createProductCommandRequest);

            return Ok(response);
        }

    }
}
