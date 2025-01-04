using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Payment;
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly ICouponReadRepository _couponReadRepository;
        private readonly IPaymentCalculatorService _paymentCalculatorService;
        private readonly IBasketReadRepository _basketReadRepository;
        private readonly IStockService _stockService;
        private readonly IAddressValidationService _addressValidationService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSession _jwtSession;

        public CheckoutService(IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, IBasketReadRepository basketReadRepository, ICouponReadRepository couponReadRepository, IPaymentCalculatorService paymentCalculatorService, IPaymentService paymentService, IUnitOfWork unitOfWork, IJwtSession jwtSession, IStockService stockService, IAddressValidationService addressValidationService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _basketReadRepository = basketReadRepository;
            _couponReadRepository = couponReadRepository;
            _paymentCalculatorService = paymentCalculatorService;
            _unitOfWork = unitOfWork;
            _jwtSession = jwtSession;
            _paymentService = paymentService;
            _stockService = stockService;
            _addressValidationService = addressValidationService;
        }

        public async Task<GetCheckoutResponseDTO> CheckoutAsync(CreateCheckoutDTO createCheckoutDTO, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var basket = await _basketReadRepository.Table
                                .Include(b => b.Items)
                                    .ThenInclude(bi => bi.Product)
                                        .ThenInclude(p => p.Discount)
                                .Include(b => b.Items)
                                    .ThenInclude(bi => bi.Product)
                                        .ThenInclude(p => p.Categories)
                                            .ThenInclude(c => c.Discount)
                                .Include(b => b.Order)
                                .Include(b => b.User)
                                    .ThenInclude(u => u.ShippingAddress)
                                .Include(b => b.User)
                                    .ThenInclude(u => u.BillingAddress)
                                .FirstOrDefaultAsync(b => b.Id == createCheckoutDTO.BasketId, cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));
            

            if (userId != basket.UserId)
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");

            Coupon? coupon = null;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!string.IsNullOrEmpty(createCheckoutDTO.CouponCode))
            {
                coupon = await _couponReadRepository.Table.FirstOrDefaultAsync(c => c.Code == createCheckoutDTO.CouponCode, cancellationToken);
            }

            foreach (var item in basket.Items)
            {
                _stockService.DeduckStock(item.Product, item.Quantity);
            }

            if (createCheckoutDTO.ShippingAddress is CreateShippingAddressDTO shippingAddress)
            {
                _addressValidationService.ValidateShippingAddress(createCheckoutDTO.ShippingAddress);

                basket.User.ShippingAddress ??= new();

                basket.User.ShippingAddress.FirstName = shippingAddress.FirstName;
                basket.User.ShippingAddress.LastName = shippingAddress.LastName;
                basket.User.ShippingAddress.City = shippingAddress.City;
                basket.User.ShippingAddress.Country = shippingAddress.Country;
                basket.User.ShippingAddress.Email = shippingAddress.Email;
                basket.User.ShippingAddress.Phone = shippingAddress.Phone;
                basket.User.ShippingAddress.PostalCode = shippingAddress.PostalCode;
                basket.User.ShippingAddress.StreetAddress = shippingAddress.StreetAddress;
            }

            if (createCheckoutDTO.BillingAddress is CreateBillingAddressDTO billingAddress)
            {

                await _addressValidationService.ValidateBillingAddressAsync(createCheckoutDTO.BillingAddress, cancellationToken);

                basket.User.BillingAddress ??= new();

                basket.User.BillingAddress.FirstName = billingAddress.FirstName;
                basket.User.BillingAddress.LastName = billingAddress.LastName;
                basket.User.BillingAddress.City = billingAddress.City;
                basket.User.BillingAddress.Country = billingAddress.Country;
                basket.User.BillingAddress.Email = billingAddress.Email;
                basket.User.BillingAddress.Phone = billingAddress.Phone;
                basket.User.BillingAddress.PostalCode = billingAddress.PostalCode;
                basket.User.BillingAddress.StreetAddress = billingAddress.StreetAddress;
            }

            basket.Order ??= new();

            basket.Order.BillingAddress = basket.User.BillingAddress;
            basket.Order.ShippingAddress = basket.User.ShippingAddress;
            basket.Order.OrderCode = IOrderService.GenerateOrderCode();
            basket.Order.OrderStatus = OrderStatus.Pending;
            basket.Order.Coupon = coupon;
            basket.Order.ContactNumber = createCheckoutDTO.Phone ?? basket.User.ShippingAddress.Phone;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return await _paymentService.CreatePaymentAsync(basket.Order, userId, createCheckoutDTO.PaymentMethod, _paymentCalculatorService.CalculatePaymentPrice(basket.Order), cancellationToken);
        }
    }
}
