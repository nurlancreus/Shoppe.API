using System;
using System.Collections.Generic;

namespace Shoppe.Application.DTOs.Payment
{
    public record PayPalWebhookNotification
    {
        public string EventType { get; set; }  // EventType should be required
        public PayPalPaymentResource Resource { get; set; }
        public List<PayPalWebhookLink> Links { get; set; } // Representing the links array in the response
    }

    public record PayPalPaymentResource
    {
        public string Id { get; set; }  // This corresponds to the resource id (e.g., authorization id)
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; } // Update time is nullable
        public string State { get; set; }  // The state of the authorization (e.g., 'authorized')
        public PayPalPaymentAmount Amount { get; set; }
        public string ParentPayment { get; set; }  // The parent payment reference
        public DateTime ValidUntil { get; set; }
        public List<PayPalWebhookLink> Links { get; set; } // Links to related resources, e.g., capture, void
    }

    public record PayPalPaymentAmount
    {
        public string Total { get; set; }
        public string Currency { get; set; }
        public PayPalPaymentAmountDetails Details { get; set; }  // Includes details like subtotal
    }

    public record PayPalPaymentAmountDetails
    {
        public string Subtotal { get; set; }
    }

    public record PayPalWebhookLink
    {
        public string Href { get; set; }  // URL to the related resource
        public string Rel { get; set; }  // Relationship type (e.g., 'self', 'capture')
        public string Method { get; set; }  // HTTP method (e.g., GET, POST)
    }
}
