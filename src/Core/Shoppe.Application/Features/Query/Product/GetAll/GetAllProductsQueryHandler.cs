using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Application.Features.Query.Category.GetAllCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        private readonly IProductService _productService;

        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAsync(request.ToProductFilterParamsDTO(), cancellationToken);

            return new GetAllProductsQueryResponse()
            {
                IsSuccess = true,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Products,
            };
        }
    }
}
