using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Coupon.Get
{
    public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQueryRequest, GetCouponByIdQueryResponse>
    {
        public Task<GetCouponByIdQueryResponse> Handle(GetCouponByIdQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
