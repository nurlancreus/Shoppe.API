using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Application.Features.Command.Product.DeleteProduct;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Features.Query.Product.GetAllProducts;
using Shoppe.Application.Features.Query.Product.GetProductById;
using Shoppe.Application.Features.Query.Product.GetProductReviews;
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
        public async Task<IActionResult> Create([FromForm] CreateProductCommandRequest createProductCommandRequest)
        {
            var response = await _mediator.Send(createProductCommandRequest);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            var response = await _mediator.Send(getAllProductsQueryRequest);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var getProductByIdQueryRequest = new GetProductByIdQueryRequest()
            {
                Id = id
            };

            var response = await _mediator.Send(getProductByIdQueryRequest);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateProductCommandRequest updateProductCommandRequest)
        {
            updateProductCommandRequest.Id = id;

            var response = await _mediator.Send(updateProductCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteProductCommandRequest = new DeleteProductCommandRequest()
            {
                Id = id
            };

            var response = await _mediator.Send(deleteProductCommandRequest);

            return Ok(response);
        }

        [HttpGet("{productId}/reviews")]
        public async Task<IActionResult> GetReviews(string productId)
        {
            var getProductReviewsRequest = new GetProductReviewsQueryRequest() { ProductId = productId };

            var response = await _mediator.Send(getProductReviewsRequest);

            return Ok(response);
        }
    }
}
