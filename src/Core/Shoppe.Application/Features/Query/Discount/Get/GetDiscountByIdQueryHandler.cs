using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Discount.Get
{
    public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQueryRequest, GetDiscountByIdQueryResponse>
    {
        public Task<GetDiscountByIdQueryResponse> Handle(GetDiscountByIdQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
