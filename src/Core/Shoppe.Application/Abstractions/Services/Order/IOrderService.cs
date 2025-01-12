using Shoppe.Application.DTOs.Order;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<GetOrderDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task ShipOrderAsync(Guid id, CancellationToken cancellationToken = default);
        Task OrderCreatedAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderShippedAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderProcessingAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderCanceledAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderFailedAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderRefundedAsync(Order order, CancellationToken cancellationToken = default);

        static string GenerateOrderCode()
        {
            return $"ORD-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}
