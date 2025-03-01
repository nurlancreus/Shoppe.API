using Shoppe.Application.DTOs.Order;
using Shoppe.Domain.Entities;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<GetOrderDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetAllOrdersDTO> GetAllAsync(int page, int size, CancellationToken cancellationToken = default);
        Task ShipOrderAsync(Guid id, CancellationToken cancellationToken = default);
        Task OrderCreatedAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderShippedAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderProcessingAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderCanceledAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderFailedAsync(Order order, CancellationToken cancellationToken = default);
        Task OrderRefundedAsync(Order order, CancellationToken cancellationToken = default);
        Task CompleteOrderAsync (Guid orderId, CancellationToken cancellationToken = default);
        static string GenerateOrderCode()
        {
            return $"ORD-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}
