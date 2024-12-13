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
        Task<GetBasketDTO?> GetMyCurrentBasketAsync(CancellationToken cancellationToken = default);
        Task ClearBasketAsync(CancellationToken cancellationToken = default);
        Task AddItemToBasketAsync(Guid productId, int? quantity, CancellationToken cancellationToken = default);
        Task UpdateItemQuantityAsync(Guid basketItemId, int quantity, CancellationToken cancellationToken = default);
        Task UpdateItemQuantityAsync(Guid basketItemId, bool increment, CancellationToken cancellationToken = default);
        Task RemoveBasketItemAsync(Guid basketItemId, CancellationToken cancellationToken = default);
        Task RemoveBasketAsync(Guid basketId, CancellationToken cancellationToken = default);
        Task RemoveCurrentBasketAsync(CancellationToken cancellationToken = default);
    }
}
