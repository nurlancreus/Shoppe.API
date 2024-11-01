using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Features.Command.Product.ChangeMainImage;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Application.Features.Command.Product.DeleteProduct;
using Shoppe.Application.Features.Command.Product.RemoveImage;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Query.Product.GetAllProducts;
using Shoppe.Application.Features.Query.Product.GetProductById;
using Shoppe.Application.Features.Query.Review.GetReviewByEntity;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class ProductsController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet]
        // [ServiceFilter(typeof(SortByProductsActionFilter))]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var request = new GetProductByIdQueryRequest()
            {
                Id = id
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateProductCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteProductCommandRequest()
            {
                Id = id
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{productId}/reviews")]
        public async Task<IActionResult> GetReviews(string productId)
        {
            var request = new GetReviewsByEntityRequest { EntityId = productId, ReviewType = ReviewType.Product };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("{productId}/reviews")]
        public async Task<IActionResult> AddReview(string productId, [FromBody] CreateReviewCommandRequest request)
        {
            request.Type = ReviewType.Product;
            request.EntityId = productId;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("colors")]
        public async Task<IActionResult> GetColors()
        {
            List<string> colors = [];

            foreach (var color in Enum.GetNames<Color>())
            {
                colors.Add(color);
            }

            return await Task.FromResult(Ok(colors));
        }

        [HttpGet("materials")]
        public async Task<IActionResult> GetMaterials()
        {
            List<string> materials = [];

            foreach (var material in Enum.GetNames<Material>())
            {
                materials.Add(material);
            }

            return await Task.FromResult(Ok(materials));
        }

        [HttpPatch("{productId}/images/{imageId}")]
        public async Task<IActionResult> ChangeMainImage(string productId, string imageId)
        {
            var request = new ChangeMainImageCommandRequest
            {
                ProductId = productId,
                ImageId = imageId
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(string productId, string imageId)
        {
            var request = new RemoveImageCommandRequest
            {
                ProductId = productId,
                ImageId = imageId
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
