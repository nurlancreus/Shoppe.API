using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Events
{
    public class PaymentCaptureCompletedEvent : INotification
    {
        public string? PaymentOrderId { get; }

        public PaymentCaptureCompletedEvent(string? paymentOrderId)
        {
            PaymentOrderId = paymentOrderId;
        }
    }
}
