using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Mail.Templates
{
    public interface IOrderEmailTemplateService
    {
        string GenerateOrderConfirmationTemplate(string recipientName, string orderNumber, Order order, decimal totalAmount);
        string GenerateShippingConfirmationTemplate(string recipientName, string trackingNumber, DateTime shippingDate);
        string GenerateOrderDeliveredTemplate(string recipientName, string orderNumber, DateTime deliveryDate);
        string GenerateInvoiceTemplate(string recipientName, string invoiceNumber, Order order, decimal totalAmount, DateTime invoiceDate);
    }
}
