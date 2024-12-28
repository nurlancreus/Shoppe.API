using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IBasketReadRepository _basketReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSession _jwtSession;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, IBasketReadRepository basketReadRepository, IUnitOfWork unitOfWork, IJwtSession jwtSession, UserManager<ApplicationUser> userManager)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _basketReadRepository = basketReadRepository;
            _unitOfWork = unitOfWork;
            _jwtSession = jwtSession;
            _userManager = userManager;
        }

        public async Task CheckoutAsync(Guid basketId, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var basket = await _basketReadRepository.Table
                                .Include(b => b.User)
                                .FirstOrDefaultAsync(b => b.Id == basketId, cancellationToken);

            if (basket == null)
            {
                throw new EntityNotFoundException(nameof(basket));
            }

            if (userId != basket.UserId)
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");

            var user = await _userManager.Users
                                .Include(u => u.ShippingAddress)
                                .Include(u => u.BillingAddress)
                                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            var order = new Order
            {
                Basket = basket,
                BillingAddress = user.BillingAddress,
                ShippingAddress = user.ShippingAddress,
                OrderStatus = OrderStatus.Pending,
                OrderCode = IOrderService.GenerateOrderCode(),
                ContactNumber = user.PhoneNumber ?? string.Empty,
                
            };

            await _orderWriteRepository.AddAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }    
    }
}
