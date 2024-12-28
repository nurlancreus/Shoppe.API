using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Order;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public OrderService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, IJwtSession jwtSession, IUnitOfWork unitOfWork, ICalculatorService calculatorService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _jwtSession = jwtSession;
            _unitOfWork = unitOfWork;
            _calculatorService = calculatorService;
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

            if (order == null)
                throw new EntityNotFoundException(nameof(order));

            return order.ToGetOrderDTO(_calculatorService);
        }

        public async Task PlaceOrderAsync(Guid id, CancellationToken cancellationToken = default)
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

            if (order == null)
                throw new EntityNotFoundException(nameof(order));

            if (order.OrderStatus != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be finalized.");

            foreach (var item in order.Basket.Items)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new InvalidOperationException($"Product '{item.Product.Name}' is out of stock.");
            }

            throw new NotImplementedException();
        }
    }
}
