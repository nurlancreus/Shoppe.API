using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.SyncBasket
{
    public class SyncBasketCommandHandler : IRequestHandler<SyncBasketCommandRequest, SyncBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public SyncBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<SyncBasketCommandResponse> Handle(SyncBasketCommandRequest request, CancellationToken cancellationToken)
        {
            var isSuccess = await _basketService.SyncBasketAsync(request.GuestBasket, cancellationToken);

            return new SyncBasketCommandResponse
            {
                IsSuccess = isSuccess,
            };
        }
    }
}
