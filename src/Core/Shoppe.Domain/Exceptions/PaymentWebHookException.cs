using Shoppe.Domain.Exceptions.Base;
using System;
using System.Net;

namespace Shoppe.Domain.Exceptions
{
    public class PaymentWebHookException : BaseException
    {
        public string WebhookEventType { get; set; } = string.Empty;
        public string? WebhookId { get; set; }

        public override string Title { get; set; } = "Payment Webhook Error";
        public override string Description { get; set; } = "An error occurred while processing the payment webhook.";

        public PaymentWebHookException() : base("An error occurred while processing the payment webhook.", HttpStatusCode.BadRequest)
        {
        }

        public PaymentWebHookException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }

        public PaymentWebHookException(string message, string webhookEventType) : base(message, HttpStatusCode.BadRequest)
        {
            WebhookEventType = webhookEventType;
        }

        public PaymentWebHookException(string message, string webhookEventType, string? webhookId) : base(message, HttpStatusCode.BadRequest)
        {
            WebhookEventType = webhookEventType;
            WebhookId = webhookId;
        }

        public PaymentWebHookException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }

        public PaymentWebHookException(string message, Exception innerException) : base(message, HttpStatusCode.BadRequest, innerException)
        {
        }

        public PaymentWebHookException(string message, string webhookEventType, Exception innerException) : base(message, HttpStatusCode.BadRequest, innerException)
        {
            WebhookEventType = webhookEventType;
        }
    }
}
