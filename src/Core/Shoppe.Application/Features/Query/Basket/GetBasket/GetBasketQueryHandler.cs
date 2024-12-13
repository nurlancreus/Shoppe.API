using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Basket.GetBasket
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQueryRequest, GetBasketQueryResponse>
    {
        private readonly IBasketService _basketService;

        public GetBasketQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<GetBasketQueryResponse> Handle(GetBasketQueryRequest request, CancellationToken cancellationToken)
        {
            var basket = await _basketService.GetMyCurrentBasketAsync(cancellationToken);

            return new GetBasketQueryResponse
            {
                IsSuccess = true,
                Data = basket
            };
        }
    }
}
