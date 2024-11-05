using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.BasketItemRepos;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Basket;
using Shoppe.Application.DTOs.User;
using Shoppe.Application.Extensions.Helpers;
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
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBasketReadRepository _basketReadRepository;
        private readonly IBasketWriteRepository _basketWriteRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;
        private readonly IBasketItemWriteRepository _basketItemWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IDiscountCalculatorService _discountCalculatorService;
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IHttpContextAccessor httpContextAccessor, IBasketReadRepository basketReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IProductReadRepository productReadRepository, IUnitOfWork unitOfWork, IDiscountCalculatorService discountCalculatorService, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _basketReadRepository = basketReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
            _discountCalculatorService = discountCalculatorService;
            _userManager = userManager;
        }

        public async Task<GetBasketDTO?> GetMyCurrentBasketAsync(CancellationToken cancellationToken)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                .Include(b => b.User)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) return null;

            return new GetBasketDTO()
            {
                Id = basket.Id,
                CreatedAt = basket.CreatedAt,
                BasketItems = basket.Items.Select(bi => new GetBasketItemDTO
                {
                    Id = bi.Id.ToString(),
                    ProductName = bi.Product.Name,
                    Price = bi.Product.Price,
                    Quantity = bi.Quantity,
                    CreatedAt = bi.CreatedAt,
                    DiscountedPrice = _discountCalculatorService.CalculateDiscountedPrice(bi.Product) ?? 0,
                    TotalPrice = bi.Quantity * bi.Product.Price,
                    TotalDiscountedPrice = bi.Quantity * (_discountCalculatorService.CalculateDiscountedPrice(bi.Product) ?? 0)
                }).ToList(),
                User = new GetUserDTO
                {
                    Id = basket.User.Id,
                    FirstName = basket.User.FirstName!,
                    LastName = basket.User.LastName!,
                    Email = basket.User.Email!,
                    UserName = basket.User.UserName!,
                    CreatedAt = basket.User.CreatedAt,
                },
                TotalAmount = basket.Items.Sum(bi => bi.Quantity * bi.Product.Price),
                TotalDiscountedAmount = basket.Items.Sum(bi => bi.Quantity * (_discountCalculatorService.CalculateDiscountedPrice(bi.Product) ?? 0))
            };
        }

        private async Task<Basket> GetMyBasketAsync(CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                throw new InvalidOperationException("No username found in the HTTP context.");

            var user = await _userManager.Users
                .Include(u => u.Baskets)
                .ThenInclude(b => b.Order)
                .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken)
                ?? throw new InvalidOperationException("User not found.");

            var targetBasket = user.Baskets
                .FirstOrDefault(basket => basket.Order == null || basket.Order.OrderStatus != OrderStatus.Completed);

            if (targetBasket == null)
            {
                targetBasket = new Basket();
                user.Baskets.Add(targetBasket);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return targetBasket;
        }



        public async Task AddItemToBasketAsync(Guid productId, int? quantity, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            var existingItemInBasket = basket!.Items.FirstOrDefault(bi => bi.ProductId == product.Id);

            if (existingItemInBasket == null)
            {

                var basketItem = new BasketItem
                {
                    Product = product,
                    Quantity = quantity ?? 1,

                };

                myBasket.Items.Add(basketItem);
            }
            else
            {
                existingItemInBasket.Quantity += quantity ?? 1;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveBasket(Guid basketId, CancellationToken cancellationToken)
        {
            var basket = await _basketReadRepository.GetByIdAsync(basketId, cancellationToken);

            if (basket == null)
            {
                throw new EntityNotFoundException(nameof(basket));
            }

            _basketWriteRepository.Delete(basket);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveBasketItemAsync(Guid basketItemId, CancellationToken cancellationToken)
        {
            var basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId, cancellationToken);

            if (basketItem == null)
            {
                throw new EntityNotFoundException(nameof(basketItem));
            }

            _basketItemWriteRepository.Delete(basketItem);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveCurrentBasket(CancellationToken cancellationToken)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            _basketWriteRepository.Delete(myBasket);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateItemQuantityAsync(Guid basketItemId, int quantity, CancellationToken cancellationToken)
        {
            var basketItem = await _basketItemReadRepository.Table
                .Include(bi => bi.Product)
                .FirstOrDefaultAsync(bi => bi.Id == basketItemId, cancellationToken);

            if (basketItem == null)
            {
                throw new EntityNotFoundException(nameof(basketItem));
            }

            basketItem.Quantity = Math.Clamp(quantity, 0, basketItem.Product.Stock);

            if (basketItem.Quantity == 0)
            {
                _basketItemWriteRepository.Delete(basketItem);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateItemQuantityAsync(Guid basketItemId, bool increment, CancellationToken cancellationToken)
        {
            var basketItem = await _basketItemReadRepository.Table
                           .Include(bi => bi.Product)
                           .FirstOrDefaultAsync(bi => bi.Id == basketItemId, cancellationToken);
            if (basketItem == null)
            {
                throw new EntityNotFoundException(nameof(basketItem));
            }

            if (increment)
            {
                if (basketItem.Quantity < basketItem.Product.Stock)
                {
                    basketItem.Quantity++;
                }
            }
            else
            {
                if (basketItem.Quantity > 0)
                {
                    basketItem.Quantity--;

                    if (basketItem.Quantity == 0)
                    {
                        _basketItemWriteRepository.Delete(basketItem);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
