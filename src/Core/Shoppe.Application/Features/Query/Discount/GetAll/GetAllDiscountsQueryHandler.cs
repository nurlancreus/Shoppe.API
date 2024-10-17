using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Discount.GetAll
{
    public class GetAllDiscountsQueryHandler : IRequestHandler<GetAllDiscountsQueryRequest, GetAllDiscountsQueryResponse>
    {
        public Task<GetAllDiscountsQueryResponse> Handle(GetAllDiscountsQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
