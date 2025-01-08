﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment
{
    public interface IStripeService
    {
        Task<(string paymentIntentId, string clientSecret)> CreatePaymentIntentAsync(long amount, string currency, CancellationToken cancellationToken = default);

        Task<bool> CapturePaymentAsync(string paymentIntentId, CancellationToken cancellationToken = default);

        Task<bool> ConfirmPaymentAsync(string paymentIntentId, CancellationToken cancellationToken = default);

        Task<bool> CancelPaymentAsync(string paymentIndentId, CancellationToken cancellationToken);
    }
}
