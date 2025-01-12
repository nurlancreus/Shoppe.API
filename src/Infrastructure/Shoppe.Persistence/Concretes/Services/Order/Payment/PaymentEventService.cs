using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.UoW;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.HubServices;

namespace Shoppe.Persistence.Concretes.Services.Payment
{
    public class PaymentEventService : IPaymentEventService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IAdminHubService _adminHubService;
        public PaymentEventService(IOrderReadRepository orderReadRepository, IUnitOfWork unitOfWork, IOrderService orderService, IAdminHubService adminHubService)
        {
            _orderReadRepository = orderReadRepository;
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _adminHubService = adminHubService;
        }

        public async Task PaymentSucceededAsync(string? transactionId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(transactionId, cancellationToken);

            await _adminHubService.PaymentSucceededMessageAsync($"Payment for order (id: {order.Id}) succeeded. Please ship the order");

            order.Payment!.Status = PaymentStatus.Completed;
            await _orderService.OrderProcessingAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task PaymentCanceledAsync(string? transactionId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(transactionId, cancellationToken);

            order.Payment!.Status = PaymentStatus.Canceled;
            await _orderService.OrderCanceledAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task PaymentFailedAsync(string? transactionId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(transactionId, cancellationToken);

            order.Payment!.Status = PaymentStatus.Failed;
            await _orderService.OrderFailedAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task PaymentRefundedAsync(string? transactionId, CancellationToken cancellationToken = default)
        {
            var order = await GetOrderAsync(transactionId, cancellationToken);

            order.Payment!.Status = PaymentStatus.Refunded;
            await _orderService.OrderRefundedAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Order> GetOrderAsync(string? transactionId, CancellationToken cancellationToken = default)
        {
            var order = await _orderReadRepository.Table
                                .Include(o => o.Payment)
                                .Include(o => o.Shipment)
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.User)
                                        .ThenInclude(u => u.BillingAddress)
                                .FirstOrDefaultAsync(o => o.Payment!.TransactionId == transactionId, cancellationToken);

            if (order == null || order.Payment == null) throw new EntityNotFoundException(nameof(order));

            return order;
        }
    }
}
