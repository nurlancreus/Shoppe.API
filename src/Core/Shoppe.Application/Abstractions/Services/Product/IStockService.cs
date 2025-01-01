using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IStockService
    {
        bool IsStockAvailable(Product product, int quantity);
        void DeduckStock (Product product, int quantity);
        void AddStock (Product product, int quantity);
        Task<bool> IsStockAvailableAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
        Task DeduckStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
        Task AddStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
    }
}
