using Microsoft.EntityFrameworkCore;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Infrastructure.Concretes.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IPayPalService _payPalService;
        private readonly IStripeService _stripeService;
        private readonly IUnitOfWork _unitOfWork;

        // TODO: get transaction ids from gateways and add cancel methods for each gateway

        public PaymentService(IOrderReadRepository orderReadRepository, IPayPalService payPalService, IStripeService stripeService, IUnitOfWork unitOfWork)
        {
            _orderReadRepository = orderReadRepository;
            _payPalService = payPalService;
            _stripeService = stripeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCheckoutResponseDTO> CreatePaymentAsync(Order order, string userId, PaymentMethod paymentMethod, double amount, CancellationToken cancellationToken = default)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (order.Basket.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to perform this action.");

            order.Payment ??= new()
            {
                Method = paymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                PaymentReference = string.Empty,
                TransactionId = string.Empty,
            };

            order.Payment.Amount = amount;

            // Save the initial state of payment
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var gatewayResponse = paymentMethod switch
            {
                PaymentMethod.PayPal => new GetCheckoutResponseDTO
                {
                    ApprovalUrl = await _payPalService.CreatePaymentAsync(amount, "USD", cancellationToken),
                    PaymentMethod = nameof(PaymentMethod.PayPal),

                },
                PaymentMethod.DebitCard => new GetCheckoutResponseDTO
                {
                    ClientSecret = await _stripeService.CreatePaymentIntentAsync((long)(amount * 100), "USD", cancellationToken),
                    PaymentMethod = nameof(PaymentMethod.DebitCard),
                },
                PaymentMethod.CashOnDelivery => new GetCheckoutResponseDTO
                {
                    Message = HandleCashOnDelivery(),
                    PaymentMethod = nameof(PaymentMethod.CashOnDelivery),
                },
                _ => throw new PaymentFailedException("Invalid payment method")
            };
            // Save the payment reference and transaction info to the order
            order.Payment.PaymentReference = gatewayResponse.ClientSecret ?? gatewayResponse.ApprovalUrl ?? Guid.NewGuid().ToString();  // Use the reference (like approval URL or paymentIntentId)

            order.Payment.TransactionId = Guid.NewGuid().ToString(); // For this example, we're just using a GUID for the TransactionId

            var paymentTransaction = new PaymentTransaction(_payPalService, _stripeService, order, _unitOfWork);

            // Enlist the payment transaction
            Transaction.Current?.EnlistVolatile(paymentTransaction, EnlistmentOptions.None);


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return gatewayResponse;
        }

        public async Task<bool> ConfirmPaymentAsync(Guid orderId, string paymentId, string payerId, CancellationToken cancellationToken = default)
        {
            var order = await _orderReadRepository.Table
                                .Include(o => o.Payment)
                                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null || order.Payment == null)
                throw new EntityNotFoundException(nameof(order));

            bool paymentConfirmed = false;

            paymentConfirmed = order.Payment.Method switch
            {
                PaymentMethod.PayPal => await _payPalService.ExecutePaymentAsync(paymentId, payerId, cancellationToken),
                PaymentMethod.DebitCard => await _stripeService.ConfirmPaymentAsync(paymentId, cancellationToken),
                PaymentMethod.CashOnDelivery => true,
                _ => throw new PaymentFailedException("Invalid payment method"),
            };

            if (paymentConfirmed)
            {
                order.Payment.PaymentStatus = PaymentStatus.Completed;
                order.Payment.TransactionId = paymentId;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                order.Payment.PaymentStatus = PaymentStatus.Failed;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return false;
            }
        }

        private static string HandleCashOnDelivery()
        {
            return "Cash on Delivery order has been successfully created. Awaiting payment on delivery.";
        }

    }
}
