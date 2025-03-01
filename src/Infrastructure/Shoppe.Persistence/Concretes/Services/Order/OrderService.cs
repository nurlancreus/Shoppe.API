using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Mail;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Order;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IPaginationService _paginationService;
        private readonly IJwtSession _jwtSession;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICalculatorService _calculatorService;
        private readonly IOrderEmailService _orderEmailService;

        public OrderService(IOrderReadRepository orderReadRepository, IJwtSession jwtSession, IUnitOfWork unitOfWork, ICalculatorService calculatorService, IOrderEmailService orderEmailService, IPaginationService paginationService)
        {
            _orderReadRepository = orderReadRepository;
            _jwtSession = jwtSession;
            _unitOfWork = unitOfWork;
            _calculatorService = calculatorService;
            _orderEmailService = orderEmailService;
            _paginationService = paginationService;
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

        public async Task<GetAllOrdersDTO> GetAllAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            var query = _orderReadRepository.Table
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
                                    .ThenInclude(b => b.User);

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, size, query, cancellationToken);

            var orders = await paginationResult.PaginatedQuery.Select(o => o.ToGetOrderDTO(_calculatorService)).ToListAsync(cancellationToken);

            return new GetAllOrdersDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Orders = orders
            };
        }

        public async Task CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            _jwtSession.IsAdmin();

            var order = await _orderReadRepository.Table
                                .Include(o => o.Payment)
                                .Include(o => o.Shipment)
                                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null) throw new EntityNotFoundException(nameof(order));

            if (order.Status != OrderStatus.Shipped)
                throw new OrderException("Order must be in 'Shipped' state to be completed.", order.Code, order.Basket.User.Id);

            if (order.Payment?.Status != PaymentStatus.Completed)
                throw new OrderException("Order payment is not completed.", order.Code, order.Basket.User.Id);

            order.Status = OrderStatus.Completed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await OrderCompletedAsync(order, cancellationToken);
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
            await _orderEmailService.SendOrderCreatedAsync(order, cancellationToken);
        }
        public async Task OrderCanceledAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderEmailService.SendOrderCanceledAsync(order, cancellationToken);

        }

        public async Task OrderFailedAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderEmailService.SendOrderFailedAsync(order, cancellationToken);

        }

        public async Task OrderRefundedAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderEmailService.SendOrderRefundedAsync(order, cancellationToken);

        }

        public async Task OrderProcessingAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderEmailService.SendOrderProcessingAsync(order, cancellationToken);

        }

        public async Task OrderShippedAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderEmailService.SendOrderShippedAsync(order, cancellationToken);

        }

        public async Task OrderCompletedAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderEmailService.SendOrderCompletedAsync(order, cancellationToken);
        }
    }
}
