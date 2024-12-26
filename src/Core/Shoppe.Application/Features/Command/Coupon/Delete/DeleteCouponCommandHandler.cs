using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Delete
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommandRequest, DeleteCouponCommandResponse>
    {
        public Task<DeleteCouponCommandResponse> Handle(DeleteCouponCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
