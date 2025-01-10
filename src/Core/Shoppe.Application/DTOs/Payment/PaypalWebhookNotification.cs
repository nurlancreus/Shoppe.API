using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shoppe.Application.DTOs.Payment
{
    public record PayPalWebhookNotification
    {
        [JsonPropertyName("event_type")]
        public string EventType { get; set; } = string.Empty;  // Required field

        [JsonPropertyName("resource")]
        public PayPalPaymentResource Resource { get; set; } = null!;

        [JsonPropertyName("links")]
        public List<PayPalWebhookLink> Links { get; set; } = []; // Represents the links array
    }

    public record PayPalPaymentResource
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;  // Resource ID (e.g., authorization ID)

        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonPropertyName("update_time")]
        public DateTime? UpdateTime { get; set; } // Nullable for optional updates

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;  // State of the authorization (e.g., 'authorized')

        [JsonPropertyName("amount")]
        public PayPalPaymentAmount Amount { get; set; } = null!;

        [JsonPropertyName("parent_payment")]
        public string ParentPayment { get; set; } = string.Empty; // Reference to the parent payment

        [JsonPropertyName("valid_until")]
        public DateTime ValidUntil { get; set; }

        [JsonPropertyName("links")]
        public List<PayPalWebhookLink> Links { get; set; } = []; // Links to related resources
    }

    public record PayPalPaymentAmount
    {
        [JsonPropertyName("total")]
        public string Total { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public PayPalPaymentAmountDetails Details { get; set; } = null!;  // Details like subtotal
    }

    public record PayPalPaymentAmountDetails
    {
        [JsonPropertyName("subtotal")]
        public string Subtotal { get; set; } = string.Empty;
    }

    public record PayPalWebhookLink
    {
        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;  // URL to the related resource

        [JsonPropertyName("rel")]
        public string Rel { get; set; } = string.Empty;  // Relationship type (e.g., 'self', 'capture')

        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;  // HTTP method (e.g., GET, POST)
    }
}
