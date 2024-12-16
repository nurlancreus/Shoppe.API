using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Features.Command.Discount.AssignDiscount;
using Shoppe.Application.Features.Command.Discount.AssignEntities;
using Shoppe.Application.Features.Command.Product.ChangeMainImage;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Application.Features.Command.Product.DeleteProduct;
using Shoppe.Application.Features.Command.Product.RemoveImage;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Query.Product.GetAllProducts;
using Shoppe.Application.Features.Query.Product.GetByIds;
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
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetProductByIdQueryRequest()
            {
                Id = id
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("ids")]
        public async Task<IActionResult> GetByIds([FromQuery] string productIds)
        {
            var request = new GetProductsByIdQueryRequest
            {
                ProductsIds = productIds
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateProductCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteProductCommandRequest()
            {
                Id = id
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{productId}/reviews")]
        public async Task<IActionResult> GetReviews(Guid productId)
        {
            var request = new GetReviewsByEntityRequest { EntityId = productId, ReviewType = ReviewType.Product };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("assign-discount/{discountId}")]
        public async Task<IActionResult> AssignDiscountToProducts (Guid discountId, [FromBody] AssignDiscountToEntitiesCommandRequest request)
        {
            request.Id = discountId;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("{productId}/reviews")]
        public async Task<IActionResult> AddReview(Guid productId, [FromBody] CreateReviewCommandRequest request)
        {
            request.Type = ReviewType.Product;
            request.EntityId = productId;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("colors")]
        public async Task<IActionResult> GetColors()
        {
            var colors = Enum.GetNames<Color>();

            return await Task.FromResult(Ok(colors));
        }

        [HttpGet("materials")]
        public async Task<IActionResult> GetMaterials()
        {
            var materials = Enum.GetNames<Material>();

            return await Task.FromResult(Ok(materials));
        }

        [HttpPatch("{productId}/images/{imageId}")]
        public async Task<IActionResult> ChangeMainImage(Guid productId, Guid imageId)
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
        public async Task<IActionResult> RemoveImage(Guid productId, Guid imageId)
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
