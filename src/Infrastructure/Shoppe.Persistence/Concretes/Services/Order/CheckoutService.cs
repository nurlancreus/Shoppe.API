using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Address;
using Shoppe.Application.DTOs.Checkout;
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
        private readonly IStockService _stockService;
        private readonly IAddressValidationService _addressValidationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSession _jwtSession;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, IBasketReadRepository basketReadRepository, IUnitOfWork unitOfWork, IJwtSession jwtSession, UserManager<ApplicationUser> userManager, IStockService stockService, IAddressValidationService addressValidationService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _basketReadRepository = basketReadRepository;
            _unitOfWork = unitOfWork;
            _jwtSession = jwtSession;
            _userManager = userManager;
            _stockService = stockService;
            _addressValidationService = addressValidationService;
        }

        public async Task CheckoutAsync(CreateCheckoutDTO createCheckoutDTO, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var basket = await _basketReadRepository.Table
                                .Include(b => b.Items)
                                    .ThenInclude(bi => bi.Product)
                                //.Include(b => b.User)
                                .FirstOrDefaultAsync(b => b.Id == createCheckoutDTO.BasketId, cancellationToken);

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

            foreach (var item in basket.Items)
            {
                _stockService.DeduckStock(item.Product, item.Quantity);
            }

            if (createCheckoutDTO.ShippingAddress is CreateShippingAddressDTO shippingAddress)
            {
                _addressValidationService.ValidateShippingAddress(createCheckoutDTO.ShippingAddress);

                user.ShippingAddress = new ShippingAddress
                {
                    FirstName = shippingAddress.FirstName,
                    LastName = shippingAddress.LastName,
                    City = shippingAddress.City,
                    Country = shippingAddress.Country,
                    Email = shippingAddress.Email,
                    Phone = shippingAddress.Phone,
                    PostalCode = shippingAddress.PostalCode,
                    StreetAddress = shippingAddress.StreetAddress,
                };
            }

            if (createCheckoutDTO.BillingAddress is CreateBillingAddressDTO billingAddress)
            {

                await _addressValidationService.ValidateBillingAddressAsync(createCheckoutDTO.BillingAddress, cancellationToken);

                user.BillingAddress = new BillingAddress
                {
                    FirstName = billingAddress.FirstName,
                    LastName = billingAddress.LastName,
                    City = billingAddress.City,
                    Country = billingAddress.Country,
                    Email = billingAddress.Email,
                    Phone = billingAddress.Phone,
                    PostalCode = billingAddress.PostalCode,
                    StreetAddress = billingAddress.StreetAddress,
                };
            }

            var order = new Order
            {
                Basket = basket,
                BillingAddress = user.BillingAddress,
                ShippingAddress = user.ShippingAddress,
                OrderStatus = OrderStatus.Pending,
                OrderCode = IOrderService.GenerateOrderCode(),
                ContactNumber = createCheckoutDTO.Phone ?? user.ShippingAddress.Phone,
            };

            await _orderWriteRepository.AddAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
