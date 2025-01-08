using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Bcpg;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Checkout;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using Shoppe.Domain.Exceptions.Shoppe.Domain.Exceptions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using PaymentMethod = Shoppe.Domain.Enums.PaymentMethod;
using PaymentEntity = Shoppe.Domain.Entities.Payment;

namespace Shoppe.Infrastructure.Concretes.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IPayPalService _payPalService;
        private readonly IStripeService _stripeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentTransaction> _logger;
        public PaymentService(IOrderReadRepository orderReadRepository, IPayPalService payPalService, IStripeService stripeService, IUnitOfWork unitOfWork, ILogger<PaymentTransaction> logger)
        {
            _orderReadRepository = orderReadRepository;
            _payPalService = payPalService;
            _stripeService = stripeService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetCheckoutResponseDTO> CreatePaymentAsync(Order order, string userId, PaymentMethod paymentMethod, double amount, CancellationToken cancellationToken = default)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (order.Basket.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to perform this action.");

            var paymentReference = Guid.NewGuid().ToString();

            order.Payment ??= new()
            {
                Method = paymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                PaymentReference = paymentReference,
                TransactionId = string.Empty,
            };

            order.Payment.Amount = amount;

            // Save the initial state of payment
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var gatewayResponse = paymentMethod switch
            {
                PaymentMethod.PayPal => await ProcessPayPalPaymentAsync(order, amount, paymentReference, cancellationToken),
                PaymentMethod.DebitCard => await ProcessStripePaymentAsync(order, amount, cancellationToken),
                PaymentMethod.CashOnDelivery => ProcessCashOnDelivery(),
                _ => throw new PaymentFailedException($"Invalid payment method: {paymentMethod}")
            };

            var paymentTransaction = new PaymentTransaction(_payPalService, _stripeService, order, _unitOfWork, _logger);

            // Enlist the payment transaction
            Transaction.Current?.EnlistVolatile(paymentTransaction, EnlistmentOptions.None);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return gatewayResponse;
        }

        public async Task<bool> CompletePaymentAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            // Fetch the order and its payment details
            var order = await _orderReadRepository.Table
                                .Include(o => o.Payment)
                                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null || order.Payment == null)
                throw new EntityNotFoundException(nameof(order));

            bool paymentCaptured = await CapturePaymentAsync(order.Payment, cancellationToken);

            if (!paymentCaptured)
            {
                order.Payment.PaymentStatus = PaymentStatus.Failed;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return false;
            }

            // Now, confirm the payment after capture
            bool paymentConfirmed = await ConfirmPaymentAsync(order.Payment, cancellationToken);

            // Set the payment status based on the confirmation result
            order.Payment.PaymentStatus = paymentConfirmed ? PaymentStatus.Completed : PaymentStatus.Failed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return paymentConfirmed;
        }

        private async Task<bool> CapturePaymentAsync(PaymentEntity payment, CancellationToken cancellationToken = default)
        {
            bool paymentConfirmed = payment.Method switch
            {
                PaymentMethod.PayPal => await _payPalService.CapturePaymentAsync(payment.TransactionId, cancellationToken),
                PaymentMethod.DebitCard => await _stripeService.CapturePaymentAsync(payment.TransactionId, cancellationToken),
                PaymentMethod.CashOnDelivery => true,
                _ => throw new PaymentFailedException("Invalid payment method"),
            };
            return paymentConfirmed;
        }

        private async Task<bool> ConfirmPaymentAsync(PaymentEntity payment, CancellationToken cancellationToken = default)
        {
            bool paymentConfirmed = payment.Method switch
            {
                PaymentMethod.PayPal => await _payPalService.ConfirmPaymentAsync(payment.TransactionId, cancellationToken),
                PaymentMethod.DebitCard => await _stripeService.ConfirmPaymentAsync(payment.TransactionId, cancellationToken),
                PaymentMethod.CashOnDelivery => true,
                _ => throw new PaymentFailedException("Invalid payment method"),
            };
            payment.PaymentStatus = paymentConfirmed ? PaymentStatus.Completed : PaymentStatus.Failed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return paymentConfirmed;
        }


        private static string HandleCashOnDelivery()
        {
            return "Cash on Delivery order has been successfully created. Awaiting payment on delivery.";
        }

        private async Task<GetCheckoutResponseDTO> ProcessPayPalPaymentAsync(Order order, double amount, string paymentReference, CancellationToken cancellationToken)
        {
            var (paymentOrderId, approvalUrl) = await _payPalService.CreatePaymentAsync(amount, "USD", paymentReference, cancellationToken);

            order.Payment!.TransactionId = paymentOrderId;

            return new GetCheckoutResponseDTO
            {
                ApprovalUrl = approvalUrl,
                PaymentMethod = nameof(PaymentMethod.PayPal)
            };
        }

        private async Task<GetCheckoutResponseDTO> ProcessStripePaymentAsync(Order order, double amount, CancellationToken cancellationToken)
        {
            var (paymentIntentId, clientSecret) = await _stripeService.CreatePaymentIntentAsync((long)(amount * 100), "USD", cancellationToken);

            order.Payment!.TransactionId = paymentIntentId;

            return new GetCheckoutResponseDTO
            {
                ClientSecret = clientSecret,
                PaymentMethod = nameof(PaymentMethod.DebitCard)
            };
        }

        private static GetCheckoutResponseDTO ProcessCashOnDelivery()
        {
            return new GetCheckoutResponseDTO
            {
                Message = HandleCashOnDelivery(),
                PaymentMethod = nameof(PaymentMethod.CashOnDelivery)
            };
        }
    }
}
