﻿using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Bcpg;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Mail;
using Shoppe.Application.Abstractions.Services.Mail.Templates;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Mail;
using Shoppe.Application.DTOs.Order;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IJwtSession _jwtSession;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICalculatorService _calculatorService;
        private readonly IOrderEmailTemplateService _orderEmailTemplateService;
        private readonly IEmailService _emailService;

        public OrderService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, IJwtSession jwtSession, IUnitOfWork unitOfWork, ICalculatorService calculatorService, IOrderEmailTemplateService orderEmailTemplateService, IEmailService emailService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _jwtSession = jwtSession;
            _unitOfWork = unitOfWork;
            _calculatorService = calculatorService;
            _orderEmailTemplateService = orderEmailTemplateService;
            _emailService = emailService;
        }

        public async Task<GetOrderDTO> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await _orderReadRepository.Table
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.Items)
                                        .ThenInclude(bi => bi.Product)
                                            .ThenInclude(p => p.Discount)
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.Items)
                                        .ThenInclude(bi => bi.Product)
                                            .ThenInclude(p => p.Categories)
                                                .ThenInclude(c => c.Discount)
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.User)
                                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order == null) throw new EntityNotFoundException(nameof(order));

            return order.ToGetOrderDTO(_calculatorService);
        }

        public async Task ShipOrderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _jwtSession.IsAdmin();

            var order = await _orderReadRepository.Table
                                .Include(o => o.Payment)
                                .Include(o => o.Shipment)
                                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order == null) throw new EntityNotFoundException(nameof(order));

            if (order.Status != OrderStatus.Processing)
                throw new OrderException("Order must be in 'Processing' state to be shipped.", order.Code, order.Basket.User.Id);

            if (order.Payment?.Status != PaymentStatus.Completed)
                throw new OrderException("Order payment is not completed.", order.Code, order.Basket.User.Id);

            // Handle Shipping logic there

            order.Status = OrderStatus.Shipped;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await OrderShippedAsync(order, cancellationToken);
        }

        public async Task OrderCreatedAsync(Order order, CancellationToken cancellationToken = default)
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
        public async Task OrderCanceledAsync(Order order, CancellationToken cancellationToken = default)
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

        public async Task OrderFailedAsync(Order order, CancellationToken cancellationToken = default)
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

        public async Task OrderRefundedAsync(Order order, CancellationToken cancellationToken = default)
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

        public async Task OrderProcessingAsync(Order order, CancellationToken cancellationToken = default)
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

        public async Task OrderShippedAsync(Order order, CancellationToken cancellationToken = default)
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

        private static string GetFullName(ApplicationUser user)
        {
            return $"{user.BillingAddress.FirstName} {user.BillingAddress.LastName}";
        }

    }
}
