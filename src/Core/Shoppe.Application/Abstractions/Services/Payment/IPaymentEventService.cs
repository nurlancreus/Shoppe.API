using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment
{
    public interface IPaymentEventService
    {
        Task PaymentSucceededAsync(string? transactionId, CancellationToken cancellationToken = default);
        Task PaymentCanceledAsync(string? transactionId, CancellationToken cancellationToken = default);
        Task PaymentFailedAsync(string? transactionId, CancellationToken cancellationToken = default);
        Task PaymentRefundedAsync(string? transactionId, CancellationToken cancellationToken = default);
    }
}
