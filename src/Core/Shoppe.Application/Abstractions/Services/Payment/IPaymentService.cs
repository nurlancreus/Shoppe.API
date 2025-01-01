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
        Task<string> CreatePaymentAsync (Guid orderId, PaymentMethod paymentMethod, CancellationToken cancellationToken = default);
    }
}
