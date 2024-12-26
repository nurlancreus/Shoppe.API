using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Create
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommandRequest, CreateCouponCommandResponse>
    {
        public Task<CreateCouponCommandResponse> Handle(CreateCouponCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
