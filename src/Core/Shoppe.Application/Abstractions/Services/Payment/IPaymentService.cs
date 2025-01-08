using Shoppe.Application.DTOs.Checkout;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment
{
    public interface IPaymentService
    {
        Task<GetCheckoutResponseDTO> CreatePaymentAsync(Order order, string userId, PaymentMethod paymentMethod, double amount, CancellationToken cancellationToken = default);

        Task<bool> CompletePaymentAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
