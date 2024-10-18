using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class BasketService : IBasketService
    {
        public GetBasketDTO MyCurrentBasket => throw new NotImplementedException();

        public Task AddItemToBasketAsync(string productId, int quantity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBasket(string basketId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBasketItemAsync(string basketItemId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateItemQuantityAsync(string basketItemId, int quantity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateItemQuantityAsync(string basketItemId, bool increment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
