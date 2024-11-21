using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Mail
{
    public interface IEmailTemplateService
    {
        string GenerateContactResponseTemplate(string recipientName, string subject, string message);
        string GeneratePasswordResetTemplate(string recipientName, string resetLink);
        string GeneratePasswordChangedTemplate(string recipientName);
        string GenerateOrderConfirmationTemplate(string recipientName, string orderNumber, Order order, decimal totalAmount);
        string GenerateShippingConfirmationTemplate(string recipientName, string trackingNumber, DateTime shippingDate);
        string GenerateOrderDeliveredTemplate(string recipientName, string orderNumber, DateTime deliveryDate);
        string GenerateAccountCreatedTemplate(string recipientName);
        string GenerateInvoiceTemplate(string recipientName, string invoiceNumber, Order order, decimal totalAmount, DateTime invoiceDate);

    }
}
