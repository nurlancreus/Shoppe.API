﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment.PayPal
{
    public interface IPayPalService
    {
        Task<(string paymentOrderId, string approvalUrl)> CreatePaymentAsync(double amount, string currency, string reference, CancellationToken cancellationToken = default);
        Task<bool> CapturePaymentAsync(string paymentOrderId, CancellationToken cancellationToken = default);
        Task<bool> IsPaymentCapturedAsync(string paymentOrderId, CancellationToken cancellationToken = default);
        Task<bool> IsPaymentOrderVoidedAsync(string paymentOrderId, CancellationToken cancellationToken = default);
        Task CancelPaymentAsync(string paymentOrderId, CancellationToken cancellationToken);
    }
}
