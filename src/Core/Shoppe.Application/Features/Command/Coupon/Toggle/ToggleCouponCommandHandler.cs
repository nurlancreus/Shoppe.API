using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Toggle
{
    public class ToggleCouponCommandHandler : IRequestHandler<ToggleCouponCommandRequest, ToggleCouponCommandResponse>
    {
        private readonly ICouponService _couponService;

        public ToggleCouponCommandHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        async Task<ToggleCouponCommandResponse> IRequestHandler<ToggleCouponCommandRequest, ToggleCouponCommandResponse>.Handle(ToggleCouponCommandRequest request, CancellationToken cancellationToken)
        {
            await _couponService.ToggleAsync((Guid)request.Id!, cancellationToken);

            return new ToggleCouponCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
