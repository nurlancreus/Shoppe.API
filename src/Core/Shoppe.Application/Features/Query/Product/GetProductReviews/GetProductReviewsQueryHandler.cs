using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetProductReviews
{
    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQueryRequest, GetProductReviewsQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductReviewsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductReviewsQueryResponse> Handle(GetProductReviewsQueryRequest request, CancellationToken cancellationToken)
        {
            var reviews = await _productService.GetReviewsByProductAsync(request.ProductId!, cancellationToken);

            return new GetProductReviewsQueryResponse()
            {
                IsSuccess = true,
                Data = reviews
            };
        }
    }
}
