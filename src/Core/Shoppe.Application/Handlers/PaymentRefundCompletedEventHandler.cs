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
    public class PaymentRefundCompletedEventHandler : INotificationHandler<PaymentRefundCompletedEvent>
    {
        private readonly IPaymentEventService _paymentEventService;

        public PaymentRefundCompletedEventHandler(IPaymentEventService paymentEventService)
        {
            _paymentEventService = paymentEventService;
        }

        public async Task Handle(PaymentRefundCompletedEvent notification, CancellationToken cancellationToken)
        {
            await _paymentEventService.PaymentRefundedAsync(notification.PaymentOrderId, cancellationToken);
        }
    }
}
