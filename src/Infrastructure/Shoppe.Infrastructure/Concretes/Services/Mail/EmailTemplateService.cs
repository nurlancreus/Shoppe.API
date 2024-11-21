using Shoppe.Domain.Entities;

namespace Shoppe.Application.Abstractions.Services.Mail
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateContactResponseTemplate(string recipientName, string subject, string message)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Thank you for reaching out regarding <strong>{subject}</strong>. Below is our response:</p>
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

        public string GenerateOrderConfirmationTemplate(string recipientName, string orderNumber, Order order, decimal totalAmount)
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
                        <p>Thank you for your order! Your order number is <strong>{orderNumber}</strong>. Below are the details of your order:</p>
                        <table border='1'>
                            <tr>
                                <th>Product</th>
                                <th>Quantity</th>
                                <th>Price</th>
                            </tr>
                            {orderDetails}
                        </table>
                        <p><strong>Total: ${totalAmount}</strong></p>
                        <br>
                        <p>Best regards,</p>
                        <p>Shoppe Team</p>
                    </body>
                </html>";
        }

        public string GenerateShippingConfirmationTemplate(string recipientName, string trackingNumber, DateTime shippingDate)
        {
            return $@"
                <html>
                    <body>
                        <p>Dear {recipientName},</p>
                        <p>Your order has been shipped! You can track your shipment using the following tracking number:</p>
                        <p><strong>{trackingNumber}</strong></p>
                        <p>Shipping Date: {shippingDate.ToString("MMMM dd, yyyy")}</p>
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
                        <p>We are pleased to inform you that your order <strong>{orderNumber}</strong> has been delivered successfully on <strong>{deliveryDate.ToString("MMMM dd, yyyy")}</strong>.</p>
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

        public string GenerateInvoiceTemplate(string recipientName, string invoiceNumber, Order order, decimal totalAmount, DateTime invoiceDate)
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
    }
}
