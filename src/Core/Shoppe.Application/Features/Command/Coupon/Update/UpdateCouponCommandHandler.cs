using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Update
{
    public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommandRequest, UpdateCouponCommandResponse>
    {
        private readonly ICouponService _couponService;

        public UpdateCouponCommandHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<UpdateCouponCommandResponse> Handle(UpdateCouponCommandRequest request, CancellationToken cancellationToken)
        {
            await _couponService.UpdateAsync(new DTOs.Coupon.UpdateCouponDTO
            {
                Id = (Guid)request.Id!,
                Code = request.Code,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                MaxUsage = request.MaxUsage,
                MinimumOrderAmount = request.MinimumOrderAmount,
            }, cancellationToken);

            return new UpdateCouponCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
