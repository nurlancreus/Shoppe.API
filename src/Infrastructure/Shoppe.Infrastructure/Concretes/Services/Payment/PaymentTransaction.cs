using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions.Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Infrastructure.Concretes.Services.Payment
{
    public class PaymentTransaction : IEnlistmentNotification
    {
        private readonly IPayPalService _payPalService;
        private readonly IStripeService _stripeService;
        private readonly Order _order;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentTransaction(IPayPalService payPalService, IStripeService stripeService, Order order, IUnitOfWork unitOfWork)
        {
            _payPalService = payPalService;
            _stripeService = stripeService;
            _order = order;
            _unitOfWork = unitOfWork;
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        public void Commit(Enlistment enlistment)
        {
            // Transaction has been successfully committed, nothing to do
            enlistment.Done();
        }

        public async void Rollback(Enlistment enlistment)
        {
            await RollbackAsync();
            enlistment.Done();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            // Cancel the payment transaction from the respective payment gateway

            switch (_order.Payment!.Method)
            {
                case PaymentMethod.PayPal:
                    await _payPalService.CancelPaymentAsync(_order.Payment.PaymentReference, cancellationToken);
                    break;
                case PaymentMethod.DebitCard:
                    await _stripeService.CancelPaymentAsync(_order.Payment.TransactionId, cancellationToken);
                    break;
                case PaymentMethod.CashOnDelivery:
                    // No cancellation for Cash on Delivery
                    break;
                default:
                    throw new PaymentFailedException("Invalid payment method for rollback");
            }

            // Optionally, mark the payment status as canceled or failed
            _order.Payment.PaymentStatus = PaymentStatus.Canceled;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
