using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Basket.GetBasket
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQueryRequest, GetBasketQueryResponse>
    {
        public Task<GetBasketQueryResponse> Handle(GetBasketQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
