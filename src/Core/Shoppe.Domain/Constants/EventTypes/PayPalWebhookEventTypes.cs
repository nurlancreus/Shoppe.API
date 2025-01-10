namespace Shoppe.Domain.Constants.EventTypes
{
    public static class PayPalWebhookEventTypes
    {
        public const string PAYMENT_CAPTURE_COMPLETED = "PAYMENT.CAPTURE.COMPLETED";
        public const string PAYMENT_CAPTURE_DECLINED = "PAYMENT.CAPTURE.DECLINED";
        public const string PAYMENT_ORDER_CANCELLED = "PAYMENT.ORDER.CANCELLED";
        public const string PAYMENT_REFUND_COMPLETED = "PAYMENT.REFUND.COMPLETED";
    }
}
