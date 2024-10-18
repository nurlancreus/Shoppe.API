using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Discount.Get
{
    public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQueryRequest, GetDiscountByIdQueryResponse>
    {
        private readonly IDiscountService _discountService;

        public GetDiscountByIdQueryHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<GetDiscountByIdQueryResponse> Handle(GetDiscountByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var discount = await _discountService.GetAsync(request.Id!, cancellationToken);

            return new GetDiscountByIdQueryResponse
            {
                IsSuccess = true,
                Data = discount,
            };
        }
    }
}
