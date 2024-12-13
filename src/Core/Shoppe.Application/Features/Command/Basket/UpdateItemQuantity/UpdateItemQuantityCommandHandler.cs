using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommandRequest, UpdateItemQuantityCommandResponse>
    {
        private readonly IBasketService _basketService;
        public UpdateItemQuantityCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<UpdateItemQuantityCommandResponse> Handle(UpdateItemQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Quantity is int quantity && request.Increment is not bool)
            {
                await _basketService.UpdateItemQuantityAsync((Guid)request.BasketItemId!, quantity, cancellationToken);

            }
            else if (request.Quantity is not int && request.Increment is bool increment)
            {
                await _basketService.UpdateItemQuantityAsync((Guid)request.BasketItemId!, increment, cancellationToken);
            }
            else throw new UpdateNotSucceedException("Cannot update the basket item");

            return new UpdateItemQuantityCommandResponse { IsSuccess = true };

        }
    }
}
