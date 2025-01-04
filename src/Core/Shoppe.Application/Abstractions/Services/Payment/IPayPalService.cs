using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment
{
    public interface IPayPalService
    {
        Task<string> CreatePaymentAsync(double amount, string currency, CancellationToken cancellationToken = default);
        Task<bool> ExecutePaymentAsync(string paymentId, string payerId, CancellationToken cancellationToken = default);
        Task CancelPaymentAsync(string paymentReference, CancellationToken cancellationToken);
    }
}
