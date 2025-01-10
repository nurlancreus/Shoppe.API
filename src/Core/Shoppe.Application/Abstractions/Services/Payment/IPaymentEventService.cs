using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment
{
    public interface IPaymentEventService
    {
        Task PaymentCapturedAsync(string? paymentOrderId, CancellationToken cancellationToken = default);
        Task PaymentCancelledAsync(string? paymentOrderId, CancellationToken cancellationToken = default);
        Task PaymentDeclinedAsync(string? paymentOrderId, CancellationToken cancellationToken = default);
        Task PaymentRefundedAsync(string? paymentOrderId, CancellationToken cancellationToken = default);
    }
}
