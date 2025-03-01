using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Coupon.Get
{
    public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQueryRequest, GetCouponByIdQueryResponse>
    {
        private readonly ICouponService _couponService;

        public GetCouponByIdQueryHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<GetCouponByIdQueryResponse> Handle(GetCouponByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _couponService.GetAsync((Guid)request.Id!, cancellationToken);

            return new GetCouponByIdQueryResponse
            {
                IsSuccess = true,
                Data = coupon
            };
        }
    }
}
