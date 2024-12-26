using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Coupon.GetAll
{
    public class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQueryRequest, GetAllCouponsQueryResponse>
    {
        public Task<GetAllCouponsQueryResponse> Handle(GetAllCouponsQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
