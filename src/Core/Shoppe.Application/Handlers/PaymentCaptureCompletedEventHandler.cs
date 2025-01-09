// Shoppe.Application/Events/Handlers/PaymentCompletedEventHandler.cs

using MediatR;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Domain.Events;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Shoppe.Application.Events.Handlers
{
    public class PaymentCaptureCompletedEventHandler : INotificationHandler<PaymentCaptureCompletedEvent>
    {
        private readonly IPaymentService _paymentService;

        public PaymentCaptureCompletedEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(PaymentCaptureCompletedEvent notification, CancellationToken cancellationToken)
        {
           await _paymentService.CompletePaymentAsync(notification.PaymentOrderId, cancellationToken);
        }
    }
}
