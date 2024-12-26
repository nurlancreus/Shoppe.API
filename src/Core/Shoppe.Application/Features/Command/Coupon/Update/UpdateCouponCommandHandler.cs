using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Update
{
    public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommandRequest, UpdateCouponCommandResponse>
    {
        public Task<UpdateCouponCommandResponse> Handle(UpdateCouponCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
