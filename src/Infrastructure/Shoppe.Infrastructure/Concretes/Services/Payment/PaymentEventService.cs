using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.UoW;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;

namespace Shoppe.Infrastructure.Concretes.Services.Payment
{
    public class PaymentEventService : IPaymentEventService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentEventService(IOrderReadRepository orderReadRepository, IUnitOfWork unitOfWork)
        {
            _orderReadRepository = orderReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PaymentCapturedAsync(string? paymentOrderId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(paymentOrderId, cancellationToken);

            order.Payment!.PaymentStatus = PaymentStatus.Completed;
            order.OrderStatus = OrderStatus.Processing;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task PaymentCancelledAsync(string? paymentOrderId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(paymentOrderId, cancellationToken);

            order.Payment!.PaymentStatus = PaymentStatus.Cancelled;
            order.OrderStatus = OrderStatus.Cancelled;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task PaymentDeclinedAsync(string? paymentOrderId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(paymentOrderId, cancellationToken);

            order.Payment!.PaymentStatus = PaymentStatus.Failed;
            order.OrderStatus = OrderStatus.Failed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task PaymentRefundedAsync(string? paymentOrderId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(paymentOrderId, cancellationToken);

            order.Payment!.PaymentStatus = PaymentStatus.Refunded;
            order.OrderStatus = OrderStatus.Refunded;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Order> GetOrderAsync(string? paymentOrderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderReadRepository.Table
                                .Include(o => o.Payment)
                                .FirstOrDefaultAsync(o => o.Payment!.TransactionId == paymentOrderId, cancellationToken);
            if (order == null || order.Payment == null) throw new EntityNotFoundException(nameof(order));

            return order;
        }
    }
}
