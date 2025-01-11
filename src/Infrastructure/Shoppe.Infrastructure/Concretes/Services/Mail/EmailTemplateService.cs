using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Mail.Templates;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;

namespace Shoppe.Application.Abstractions.Services.Mail
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly ICalculatorService _calculatorService;

        public EmailTemplateService(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        public string GenerateContactResponseTemplate(string recipientName, ContactSubject subject, string message)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Thank you for reaching out regarding <strong>{StringHelpers.SplitAndJoinString(subject.ToString(), '_', ' ')}</strong>. Below is our response:</p>
                        <p>{message}</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GeneratePasswordResetTemplate(string recipientName, string resetLink)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>We received a request to reset your password. To proceed, please click the link below:</p>
                        <p><a href='{resetLink}'>Reset Password</a></p>
                        <p>If you did not request this, please ignore this email.</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GeneratePasswordChangedTemplate(string recipientName)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Your password has been successfully changed. If you did not make this change, please contact us immediately.</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GenerateOrderConfirmationTemplate(string recipientName, string orderNumber, Order order)
        {
            var orderDetails = string.Join("", order.Basket.Items.Select(item =>
            {
                var (discountedPrice, discountPercentage) = _calculatorService.CalculateDiscountedPrice(item.Product);
                var discountedPriceText = discountedPrice.HasValue ? $"${discountedPrice.Value}" : $"${item.Product.Price}";
                var discountPercentageText = discountPercentage.HasValue ? $"{discountPercentage.Value}%" : "No discount";

                return $@"
            <tr>
                <td>{item.Product.Name}</td>
                <td>{item.Quantity}</td>
                <td>${item.Product.Price}</td>
                <td>{discountedPriceText}</td>
                <td>{discountPercentageText}</td>
            </tr>
        ";
            }));

            var shippingCost = order.Shipment!.Cost;
            var basketTotal = _calculatorService.CalculateTotalDiscountedBasketItemsPrice(order.Basket);
            var basketFinalAmount = _calculatorService.CalculateCouponAppliedPrice(order.Basket);
            var orderFinalAmount = _calculatorService.CalculateCouponAppliedPrice(order);

            var basketCouponDiscount = order.Basket.Coupon != null && order.Basket.Coupon.IsActive
                ? $"{order.Basket.Coupon.DiscountPercentage}%"
                : "No basket coupon";

            var orderCouponDiscount = order.Coupon != null && order.Coupon.IsActive
                ? $"{order.Coupon.DiscountPercentage}%"
                : "No order coupon";

            var basketCouponPrice = order.Basket.Coupon != null && order.Basket.Coupon.IsActive
                ? $"${basketFinalAmount}"
                : $"${basketTotal}";

            var finalAmount = order.Coupon != null && order.Coupon.IsActive ? $"${orderFinalAmount}" : basketCouponPrice;

            return $@"
        <html>
            <body>
                <p>Dear {recipientName},</p>
                <p>Thank you for your order! Your order number is <strong>{orderNumber}</strong>. Below are the details of your order:</p>
                <table border='1'>
                    <tr>
                        <th>Product</th>
                        <th>Quantity</th>
                        <th>Price</th>
                        <th>Discounted Price</th>
                        <th>Discount Percentage</th>
                    </tr>
                    {orderDetails}
                </table>
                <p><strong>Total Before Basket Coupon: ${basketTotal}</strong></p>
                <p><strong>Basket Coupon Applied: {basketCouponDiscount}</strong></p>
                <p><strong>Total After Basket Coupon: {basketCouponPrice}</strong></p>
                <p><strong>Order Coupon Applied: {orderCouponDiscount}</strong></p>
                <p><strong>Total After Order Coupon: {finalAmount}</strong></p>
                <p><strong>Shipping Cost: ${shippingCost}</strong></p>
                <p><strong>Total: ${orderFinalAmount}</strong></p>
                <br>
                <p>Best regards,</p>
                <p>Shoppe Team</p>
            </body>
        </html>";
        }

        public string GenerateOrderCanceledTemplate(string recipientName, string orderNumber, Order order)
        {
            return $@"
        <html>
            <body>
                <p>Dear {recipientName},</p>
                <p>We regret to inform you that your order with order number <strong>{orderNumber}</strong> has been canceled.</p>
                <p>If you believe this is an error, please contact our customer support team for further assistance.</p>
                <br>
                <p>Best regards,</p>
                <p>Shoppe Team</p>
            </body>
        </html>";
        }

        public string GenerateOrderFailedTemplate(string recipientName, string orderNumber, Order order)
        {
            return $@"
        <html>
            <body>
                <p>Dear {recipientName},</p>
                <p>We are sorry to inform you that your order with order number <strong>{orderNumber}</strong> has failed.</p>
                <p>This could be due to payment processing issues or other factors. Please try again later, or contact our support team if you need assistance.</p>
                <br>
                <p>Best regards,</p>
                <p>Shoppe Team</p>
            </body>
        </html>";
        }

        public string GenerateOrderRefundedTemplate(string recipientName, string orderNumber, Order order)
        {
            return $@"
        <html>
            <body>
                <p>Dear {recipientName},</p>
                <p>We would like to inform you that your order with order number <strong>{orderNumber}</strong> has been refunded.</p>
                <p>The refund will be processed to your original payment method. Please allow a few business days for the refund to appear in your account.</p>
                <br>
                <p>Best regards,</p>
                <p>Shoppe Team</p>
            </body>
        </html>";
        }

        public string GenerateOrderShippedTemplate(string recipientName, string trackingNumber, DateTime shippingDate)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Your order has been shipped! You can track your shipment using the following tracking number:</p>
                        <p><strong>{trackingNumber}</strong></p>
                        <p>Shipping Date: {shippingDate:MMMM dd, yyyy}</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GenerateOrderDeliveredTemplate(string recipientName, string orderNumber, DateTime deliveryDate)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>We are pleased to inform you that your order <strong>{orderNumber}</strong> has been delivered successfully on <strong>{deliveryDate:MMMM dd, yyyy}</strong>.</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GenerateAccountCreatedTemplate(string recipientName)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Congratulations! Your account has been successfully created with Shoppe. We are excited to have you on board.</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GenerateInvoiceTemplate(string recipientName, string invoiceNumber, Order order, double totalAmount, DateTime invoiceDate)
        {
            var orderDetails = string.Join("", order.Basket.Items.Select(item => $@"
                <tr>
                    <td>{item.Product.Name}</td>
                    <td>{item.Quantity}</td>
                    <td>${item.Product.Price}</td>
                </tr>
            "));

            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Your invoice number is <strong>{invoiceNumber}</strong>. Below are the details of your purchase:</p>
                        <table border='1'>
                            <tr>
                                <th>Product</th>
                                <th>Quantity</th>
                                <th>Price</th>
                            </tr>
                            {orderDetails}
                        </table>
                        <p><strong>Total: ${totalAmount}</strong></p>
                        <p>Invoice Date: {invoiceDate:MMMM dd, yyyy}</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GenerateContactReceivedTemplate(string recipientName, ContactSubject subject)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Thank you for reaching out regarding <strong>{StringHelpers.SplitAndJoinString(subject.ToString(), '_', ' ')}</strong></p>
                        <p>We receive your message, please wait for our response</p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }
    }
}
