using Shoppe.Application.Abstractions.Services.Mail;
using Shoppe.Application.Abstractions.Services.Mail.Templates;
using Shoppe.Application.DTOs.Mail;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Mail
{
    public class OrderEmailService : IOrderEmailService
    {
        private readonly IEmailService _emailService;
        private readonly IOrderEmailTemplateService _orderEmailTemplateService;

        public OrderEmailService(IEmailService emailService, IOrderEmailTemplateService orderEmailTemplateService)
        {
            _emailService = emailService;
            _orderEmailTemplateService = orderEmailTemplateService;
        }

        public async Task SendOrderCreatedAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            string body = _orderEmailTemplateService.GenerateOrderConfirmationTemplate(recipientName, order.Code, order);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Confirmation", body);
        }
        public async Task SendOrderCanceledAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            order.Status = OrderStatus.Canceled;

            string body = _orderEmailTemplateService.GenerateOrderCanceledTemplate(recipientName, order.Code, order);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Canceled", body);
        }

        public async Task SendOrderFailedAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            order.Status = OrderStatus.Failed;

            string body = _orderEmailTemplateService.GenerateOrderFailedTemplate(recipientName, order.Code, order);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Failed", body);
        }

        public async Task SendOrderRefundedAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            order.Status = OrderStatus.Refunded;

            string body = _orderEmailTemplateService.GenerateOrderRefundedTemplate(recipientName, order.Code, order);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Refunded", body);
        }

        public async Task SendOrderProcessingAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            order.Status = OrderStatus.Processing;

            string body = _orderEmailTemplateService.GenerateOrderProcessingTemplate(recipientName, order.Shipment!.TrackingNumber, order.Shipment.EstimatedDeliveryDate);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Processing", body);
        }

        public async Task SendOrderShippedAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            order.Status = OrderStatus.Shipped;

            string body = _orderEmailTemplateService.GenerateOrderShippedTemplate(recipientName, order.Shipment!.TrackingNumber, order.Shipment.EstimatedDeliveryDate);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Shipped", body);
        }

        public async Task SendOrderCompletedAsync(Order order, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(order.Basket.User);
            order.Status = OrderStatus.Shipped;

            string body = _orderEmailTemplateService.GenerateOrderDeliveredTemplate(recipientName, order.Shipment!.TrackingNumber, order.Shipment.EstimatedDeliveryDate);

            var recipientDetails = new RecipientDetailsDTO
            {
                Email = order.Basket.User.BillingAddress.Email!,
                Name = recipientName
            };

            await _emailService.SendEmailAsync(recipientDetails, "Order Delivered", body);
        }

        private static string GetFullName(ApplicationUser user)
        {
            return $"{user.BillingAddress.FirstName} {user.BillingAddress.LastName}";
        }
    }
}
