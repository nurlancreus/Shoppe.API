using MediatR;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Handlers
{
    public class PaymentCaptureDeclinedEventHandler : INotificationHandler<PaymentCaptureDeclinedEvent>
    {
        private readonly IPaymentEventService _paymentEventService;

        public PaymentCaptureDeclinedEventHandler(IPaymentEventService paymentEventService)
        {
            _paymentEventService = paymentEventService;
        }

        public async Task Handle(PaymentCaptureDeclinedEvent notification, CancellationToken cancellationToken)
        {
            await _paymentEventService.PaymentDeclinedAsync(notification.PaymentOrderId, cancellationToken);
        }
    }
}
