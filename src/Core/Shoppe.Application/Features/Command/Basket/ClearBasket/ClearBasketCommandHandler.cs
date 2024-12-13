using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.ClearBasket
{
    public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommandRequest, ClearBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public ClearBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<ClearBasketCommandResponse> Handle(ClearBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.ClearBasketAsync(cancellationToken);

            return new ClearBasketCommandResponse { IsSuccess = true };
        }
    }
}
