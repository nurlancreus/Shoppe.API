using Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Address;
using Shoppe.Application.DTOs.Checkout;
using Shoppe.Application.DTOs.Shipment;
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
        private readonly IPaymentCalculatorService _paymentCalculatorService;
        private readonly IOrderService _orderService;
        private readonly ICouponService _couponService;
        private readonly IBasketReadRepository _basketReadRepository;
        private readonly IStockService _stockService;
        private readonly IAddressValidationService _addressValidationService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSession _jwtSession;

        public CheckoutService(IBasketReadRepository basketReadRepository, ICouponService couponService, IPaymentCalculatorService paymentCalculatorService, IOrderService orderService, IPaymentService paymentService, IUnitOfWork unitOfWork, IJwtSession jwtSession, IStockService stockService, IAddressValidationService addressValidationService)
        {
            _basketReadRepository = basketReadRepository;
            _couponService = couponService;
            _orderService = orderService;
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
                .Include(b => b.User)
                    .ThenInclude(u => u.BillingAddress)
                .Include(b => b.User)
                    .ThenInclude(u => u.ShippingAddress)
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                        .ThenInclude(p => p.Discount)
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                        .ThenInclude(p => p.Categories)
                            .ThenInclude(c => c.Discount)
                .Include(b => b.Order)
                    .ThenInclude(o => o!.Shipment)
                .Include(b => b.Order)
                    .ThenInclude(o => o!.Coupon)
                .Include(b => b.Order)
                    .ThenInclude(o => o!.Payment)
                .FirstOrDefaultAsync(b => b.Id == createCheckoutDTO.BasketId && (b.Order == null || b.Order.Status != OrderStatus.Completed), cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));
            if (userId != basket.UserId) throw new UnauthorizedAccessException("You do not have permission to perform this action.");

            if (basket.User.ShippingAddress == null && createCheckoutDTO.ShippingAddress == null)
                throw new ValidationException("Shipping address is required");

            if (basket.User.BillingAddress == null && createCheckoutDTO.BillingAddress == null)
                throw new ValidationException("Billing address is required");

            if (basket.Order?.Shipment == null && createCheckoutDTO.Shipment == null)
                throw new ValidationException("Shipment details is required");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var item in basket.Items)
            {
                _stockService.DeductStock(item.Product, item.Quantity);
            }

            UpdateAddress(basket.User, createCheckoutDTO);

            InitializeOrder(basket, createCheckoutDTO);

            if (!string.IsNullOrEmpty(createCheckoutDTO.CouponCode))
            {
                await _couponService.ApplyCouponToOrderAsync(basket.Order!, createCheckoutDTO.CouponCode, cancellationToken);
            }

            await _orderService.OrderCreatedAsync(basket.Order!, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();

            var totalPaymentAmount = _paymentCalculatorService.CalculatePaymentPrice(basket.Order!);
            var paymentResult = await _paymentService.CreatePaymentAsync(basket.Order!, userId, createCheckoutDTO.PaymentMethod, totalPaymentAmount, cancellationToken);

            return paymentResult;
        }

        private async void UpdateAddress(ApplicationUser user, CreateCheckoutDTO dto)
        {
            if (dto.ShippingAddress is CreateShippingAddressDTO shippingAddress)
            {
                _addressValidationService.ValidateShippingAddress(shippingAddress);

                user.ShippingAddress ??= new();
                user.ShippingAddress.FirstName = shippingAddress.FirstName;
                user.ShippingAddress.LastName = shippingAddress.LastName;
                user.ShippingAddress.City = shippingAddress.City;
                user.ShippingAddress.Country = shippingAddress.Country;
                user.ShippingAddress.Email = shippingAddress.Email;
                user.ShippingAddress.Phone = shippingAddress.Phone;
                user.ShippingAddress.PostalCode = shippingAddress.PostalCode;
                user.ShippingAddress.StreetAddress = shippingAddress.StreetAddress;

            }

            if (dto.BillingAddress is CreateBillingAddressDTO billingAddressDTO)
            {
                await _addressValidationService.ValidateBillingAddressAsync(billingAddressDTO);

                user.BillingAddress ??= new();
                user.BillingAddress.FirstName = billingAddressDTO.FirstName;
                user.BillingAddress.LastName = billingAddressDTO.LastName;
                user.BillingAddress.City = billingAddressDTO.City;
                user.BillingAddress.Country = billingAddressDTO.Country;
                user.BillingAddress.Email = billingAddressDTO.Email;
                user.BillingAddress.Phone = billingAddressDTO.Phone;
                user.BillingAddress.PostalCode = billingAddressDTO.PostalCode;
                user.BillingAddress.StreetAddress = billingAddressDTO.StreetAddress;

            }
        }

        private static void InitializeOrder(Basket basket, CreateCheckoutDTO dto)
        {
            basket.Order ??= new()
            {
                Code = IOrderService.GenerateOrderCode()
            };

            basket.Order.Status = OrderStatus.Pending;
            basket.Order.ShippingAddress = basket.User.ShippingAddress;
            basket.Order.BillingAddress = basket.User.BillingAddress;
            basket.Order.ContactNumber = dto.Phone ?? basket.User.ShippingAddress.Phone;

            if (!string.IsNullOrEmpty(dto.OrderNote)) basket.Order.Note = dto.OrderNote;

            if (dto.Shipment is CreateShipmentDTO shipment)
            {
                basket.Order.Shipment ??= new Shipment
                {
                    Provider = Enum.Parse<ShippingProvider>(shipment.Provider),
                    Method = Enum.Parse<DeliveryMethod>(shipment.Method),
                    Status = ShippingStatus.Pending,
                    IsShipped = false
                };
            }
        }
    }
}
