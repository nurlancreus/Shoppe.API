using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Discount.GetAll
{
    public class GetAllDiscountsQueryHandler : IRequestHandler<GetAllDiscountsQueryRequest, GetAllDiscountsQueryResponse>
    {
        private readonly IDiscountService _discountService;

        public GetAllDiscountsQueryHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<GetAllDiscountsQueryResponse> Handle(GetAllDiscountsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _discountService.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllDiscountsQueryResponse
            {
                IsSuccess = true,
                PageSize = result.PageSize,
                Page = result.Page,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Discounts,
            };
        }
    }
}
