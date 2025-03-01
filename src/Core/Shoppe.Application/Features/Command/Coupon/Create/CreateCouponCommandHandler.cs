using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Create
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommandRequest, CreateCouponCommandResponse>
    {
        private readonly ICouponService _couponService;

        public CreateCouponCommandHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<CreateCouponCommandResponse> Handle(CreateCouponCommandRequest request, CancellationToken cancellationToken)
        {
            await _couponService.CreateAsync(new DTOs.Coupon.CreateCouponDTO
            {
                Code = request.Code,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                MaxUsage = request.MaxUsage,
                MinimumOrderAmount = request.MinimumOrderAmount
            }, cancellationToken);

            return new CreateCouponCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
