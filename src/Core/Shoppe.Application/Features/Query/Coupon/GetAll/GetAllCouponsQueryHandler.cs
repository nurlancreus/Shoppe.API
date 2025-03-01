using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Coupon.GetAll
{
    public class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQueryRequest, GetAllCouponsQueryResponse>
    {
        private readonly ICouponService _couponService;

        public GetAllCouponsQueryHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<GetAllCouponsQueryResponse> Handle(GetAllCouponsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _couponService.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllCouponsQueryResponse
            {
                IsSuccess = true,
                PageSize = result.PageSize,
                Page = result.Page,
                Data = result.Coupons,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
            };
        }
    }
}
