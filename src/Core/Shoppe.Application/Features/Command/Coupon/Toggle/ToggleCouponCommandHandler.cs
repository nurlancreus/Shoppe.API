using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Toggle
{
    public class ToggleCouponCommandHandler : IRequestHandler<ToggleCouponCommandRequest, ToggleCouponCommandResponse>
    {
        Task<ToggleCouponCommandResponse> IRequestHandler<ToggleCouponCommandRequest, ToggleCouponCommandResponse>.Handle(ToggleCouponCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
