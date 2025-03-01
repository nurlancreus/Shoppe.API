using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Delete
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommandRequest, DeleteCouponCommandResponse>
    {
        private readonly ICouponService _couponService;

        public DeleteCouponCommandHandler(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<DeleteCouponCommandResponse> Handle(DeleteCouponCommandRequest request, CancellationToken cancellationToken)
        {
            await _couponService.DeleteAsync((Guid)request.Id!, cancellationToken);

            return new DeleteCouponCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
