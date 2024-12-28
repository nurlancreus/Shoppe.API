using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Apply
{
    public class ApplyCouponCommandHandler : IRequestHandler<ApplyCouponCommandRequest, ApplyCouponCommandResponse>
    {
        private readonly ICouponService _couponService;

        public ApplyCouponCommandHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<ApplyCouponCommandResponse> Handle(ApplyCouponCommandRequest request, CancellationToken cancellationToken)
        {

            await _couponService.ApplyCouponAsync((CouponTarget)request.CouponTarget!, request.CouponCode!, cancellationToken);

            return new ApplyCouponCommandResponse
            {
                IsSuccess = true,
            };


        }
    }
}
