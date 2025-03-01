using Shoppe.Application.Abstractions.Services.Mail.Templates;
using Shoppe.Application.DTOs.Mail;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;

namespace Shoppe.Application.Abstractions.Services.Mail
{
    public interface IOrderEmailService
    {
        Task SendOrderCreatedAsync(Order order, CancellationToken cancellationToken = default);
        Task SendOrderCanceledAsync(Order order, CancellationToken cancellationToken = default);
        Task SendOrderFailedAsync(Order order, CancellationToken cancellationToken = default);
        Task SendOrderRefundedAsync(Order order, CancellationToken cancellationToken = default);
        Task SendOrderProcessingAsync(Order order, CancellationToken cancellationToken = default);
        Task SendOrderShippedAsync(Order order, CancellationToken cancellationToken = default);
        Task SendOrderCompletedAsync(Order order, CancellationToken cancellationToken = default);
    }
}
