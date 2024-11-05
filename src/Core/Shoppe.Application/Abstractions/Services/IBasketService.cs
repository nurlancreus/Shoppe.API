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
        Task<GetBasketDTO?> GetMyCurrentBasketAsync(CancellationToken cancellationToken);
        Task AddItemToBasketAsync(Guid productId, int? quantity, CancellationToken cancellationToken);
        Task UpdateItemQuantityAsync(Guid basketItemId, int quantity, CancellationToken cancellationToken);
        Task UpdateItemQuantityAsync(Guid basketItemId, bool increment, CancellationToken cancellationToken);
        Task RemoveBasketItemAsync(Guid basketItemId, CancellationToken cancellationToken);
        Task RemoveBasket(Guid basketId, CancellationToken cancellationToken);
        Task RemoveCurrentBasket(CancellationToken cancellationToken);
    }
}
