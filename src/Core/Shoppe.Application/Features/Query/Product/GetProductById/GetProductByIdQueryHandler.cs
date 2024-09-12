using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Features.Query.Category.GetCategoryById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductAsync(request.Id, cancellationToken);

            return new GetProductByIdQueryResponse()
            {
                IsSuccess = true,
                Data = product
            };
        }
    }
}
