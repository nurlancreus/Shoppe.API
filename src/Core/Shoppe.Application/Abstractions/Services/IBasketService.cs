using Shoppe.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IBasketService
    {
        GetBasketDTO MyCurrentBasket { get; }
        Task AddItemToBasketAsync(string productId, int quantity, CancellationToken cancellationToken);
        Task UpdateItemQuantityAsync(string basketItemId, int quantity, CancellationToken cancellationToken);
        Task UpdateItemQuantityAsync(string basketItemId, bool increment, CancellationToken cancellationToken);
        Task RemoveBasketItemAsync(string basketItemId);
        Task RemoveBasket(string basketId);
    }
}
