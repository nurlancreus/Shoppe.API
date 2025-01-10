﻿using MediatR;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Handlers
{
    public class PaymentCaptureCompletedEventHandler : INotificationHandler<PaymentCaptureCompletedEvent>
    {
        private readonly IPaymentEventService _paymentEventService;

        public PaymentCaptureCompletedEventHandler(IPaymentEventService paymentEventService)
        {
            _paymentEventService = paymentEventService;
        }

        public async Task Handle(PaymentCaptureCompletedEvent notification, CancellationToken cancellationToken)
        {
            await _paymentEventService.PaymentCapturedAsync(notification.PaymentOrderId, cancellationToken);
        }
    }
}
