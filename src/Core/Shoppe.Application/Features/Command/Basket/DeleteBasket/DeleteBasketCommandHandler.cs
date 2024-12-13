using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.DeleteBasket
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommandRequest, DeleteBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public DeleteBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<DeleteBasketCommandResponse> Handle(DeleteBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.RemoveBasketAsync((Guid)request.Id!, cancellationToken);

            return new DeleteBasketCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
