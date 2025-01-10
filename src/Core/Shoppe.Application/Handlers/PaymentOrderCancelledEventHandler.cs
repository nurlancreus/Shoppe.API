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
    public class PaymentOrderCancelledEventHandler : INotificationHandler<PaymentOrderCancelledEvent>
    {
        private readonly IPaymentEventService _paymentEventService;

        public PaymentOrderCancelledEventHandler(IPaymentEventService paymentEventService)
        {
            _paymentEventService = paymentEventService;
        }

        public async Task Handle(PaymentOrderCancelledEvent notification, CancellationToken cancellationToken)
        {
            await _paymentEventService.PaymentCancelledAsync(notification.PaymentOrderId, cancellationToken);
        }
    }
}
