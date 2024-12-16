using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetByIds
{
    public class GetProductsByIdQueryHandler : IRequestHandler<GetProductsByIdQueryRequest, GetProductsByIdQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductsByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductsByIdQueryResponse> Handle(GetProductsByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var productIds = request.ProductsIds!.Split(';');

            var products = await _productService.GetProductsById(productIds.Select(id => new Guid(id)), cancellationToken);

            return new GetProductsByIdQueryResponse
            {
                IsSuccess = true,
                Data = products,
            };
        }
    }
}
