using Microsoft.Extensions.Logging;
using Shoppe.Application.Abstractions.Services.Payment.PayPal;
using Shoppe.Application.Abstractions.Services.Payment.Stripe;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions.Shoppe.Domain.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services.Payment
{
    public class PaymentTransaction : IEnlistmentNotification
    {
        private readonly IPayPalService _payPalService;
        private readonly IStripeService _stripeService;
        private readonly Order _order;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentTransaction> _logger;

        public PaymentTransaction(IPayPalService payPalService, IStripeService stripeService, Order order, IUnitOfWork unitOfWork, ILogger<PaymentTransaction> logger)
        {
            _payPalService = payPalService;
            _stripeService = stripeService;
            _order = order;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void InDoubt(Enlistment enlistment)
        {
            _logger.LogWarning("Transaction is in doubt. Manual intervention may be required.");
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            _logger.LogInformation("Preparing transaction for order ID {OrderId}", _order.Id);
            preparingEnlistment.Prepared();
        }

        public void Commit(Enlistment enlistment)
        {
            _logger.LogInformation("Transaction successfully committed for order ID {OrderId}", _order.Id);
            enlistment.Done();
        }

        public async void Rollback(Enlistment enlistment)
        {
            _logger.LogWarning("Transaction rollback initiated for order ID {OrderId}", _order.Id);

            try
            {
                await RollbackAsync();
                _logger.LogInformation("Transaction successfully rolled back for order ID {OrderId}", _order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during transaction rollback for order ID {OrderId}", _order.Id);
            }

            enlistment.Done();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_order.Payment == null)
            {
                _logger.LogError("Payment information is missing for order ID {OrderId}. Cannot rollback transaction.", _order.Id);
                throw new PaymentFailedException("Payment information is missing for rollback");
            }

            try
            {
                switch (_order.Payment.Method)
                {
                    case PaymentMethod.PayPal:
                        _logger.LogInformation("Cancelling PayPal payment with reference {PaymentReference}", _order.Payment.Reference);
                        await _payPalService.CancelPaymentAsync(_order.Payment.TransactionId, cancellationToken);
                        break;

                    case PaymentMethod.DebitCard:
                        _logger.LogInformation("Cancelling Stripe payment with transaction ID {TransactionId}", _order.Payment.TransactionId);
                        await _stripeService.CancelPaymentAsync(_order.Payment.TransactionId, cancellationToken);
                        break;

                    case PaymentMethod.CashOnDelivery:
                        _logger.LogInformation("No cancellation required for Cash on Delivery for order ID {OrderId}", _order.Id);
                        break;

                    default:
                        _logger.LogError("Invalid payment method for order ID {OrderId}", _order.Id);
                        throw new PaymentFailedException("Invalid payment method for rollback");
                }

                // Update payment status
                _order.Payment.Status = PaymentStatus.Canceled;
                _order.Status = OrderStatus.Canceled;
                _logger.LogInformation("Payment status set to 'Canceled' for order ID {OrderId}", _order.Id);

                // Persist changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Changes persisted to the database for order ID {OrderId}", _order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during payment rollback for order ID {OrderId}", _order.Id);
                throw;
            }
        }
    }
}
