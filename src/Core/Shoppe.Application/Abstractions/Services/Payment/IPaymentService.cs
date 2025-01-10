using Shoppe.Application.DTOs.Checkout;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;

namespace Shoppe.Application.Abstractions.Services.Payment
{
    public interface IPaymentService
    {
        Task<GetCheckoutResponseDTO> CreatePaymentAsync(Order order, string userId, PaymentMethod paymentMethod, double amount, CancellationToken cancellationToken = default);

        Task<bool> CapturePaymentAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
