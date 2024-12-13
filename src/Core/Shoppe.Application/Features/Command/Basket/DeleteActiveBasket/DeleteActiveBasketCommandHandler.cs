using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.DeleteActiveBasket
{
    public class DeleteActiveBasketCommandHandler : IRequestHandler<DeleteActiveBasketCommandRequest, DeleteActiveBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public DeleteActiveBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<DeleteActiveBasketCommandResponse> Handle(DeleteActiveBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.RemoveCurrentBasketAsync(cancellationToken);

            return new DeleteActiveBasketCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
