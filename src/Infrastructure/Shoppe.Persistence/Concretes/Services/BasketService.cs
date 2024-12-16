using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.BasketItemRepos;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Basket;
using Shoppe.Application.DTOs.User;
using Shoppe.Application.Extensions.Helpers;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class BasketService : IBasketService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBasketReadRepository _basketReadRepository;
        private readonly IBasketWriteRepository _basketWriteRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;
        private readonly IBasketItemWriteRepository _basketItemWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IDiscountCalculatorService _discountCalculatorService;
        private readonly IJwtSession _jwtSession;
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IBasketReadRepository basketReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IProductReadRepository productReadRepository, IUnitOfWork unitOfWork, IDiscountCalculatorService discountCalculatorService, UserManager<ApplicationUser> userManager, IJwtSession jwtSession)
        {
            _basketReadRepository = basketReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
            _discountCalculatorService = discountCalculatorService;
            _userManager = userManager;
            _jwtSession = jwtSession;
        }

        public async Task<GetBasketDTO?> GetMyCurrentBasketAsync(CancellationToken cancellationToken = default)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.ProductImageFiles)
                 .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.Discount)
                 .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.Categories)
                    .ThenInclude(c => c.Discount)
                .Include(b => b.User)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) return null;

            return basket.ToGetBasketDTO(_discountCalculatorService);
        }

        private async Task<Basket> GetMyBasketAsync(CancellationToken cancellationToken = default)
        {
            var userName = _jwtSession.GetUserName();

            var user = await _userManager.Users
                .Include(u => u.Baskets)
                .ThenInclude(b => b.Order)
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken)
                ?? throw new EntityNotFoundException("User not found.");

            var targetBasket = user.Baskets
                .SingleOrDefault(basket => basket.Order == null || basket.Order.OrderStatus != OrderStatus.Completed);

            if (targetBasket == null)
            {
                targetBasket = new Basket();
                user.Baskets.Add(targetBasket);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return targetBasket;
        }


        public async Task AddItemToBasketAsync(Guid productId, int? quantity, CancellationToken cancellationToken = default)
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

            if (basket == null)
            {
                throw new EntityNotFoundException(nameof(basket));
            }

            var existingItemInBasket = basket.Items.FirstOrDefault(bi => bi.ProductId == product.Id);

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveBasketAsync(Guid basketId, CancellationToken cancellationToken = default)
        {
            var basket = await _basketReadRepository.Table.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == basketId, cancellationToken);

            if (basket == null)
            {
                throw new EntityNotFoundException(nameof(basket));
            }

            var userId = _jwtSession.GetUserId();

            if (userId != basket.User.Id) throw new DeleteNotSucceedException("Basket is not belongs to the user");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _basketWriteRepository.Delete(basket);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task RemoveBasketItemAsync(Guid basketItemId, CancellationToken cancellationToken = default)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                               .Include(b => b.Items)
                               .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));

            if (basket.Items.Count > 0)
            {
                var basketItem = basket.Items.FirstOrDefault(bi => bi.Id == basketItemId);

                if (basketItem == null)
                {
                    throw new EntityNotFoundException(nameof(basketItem));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                basket.Items.Remove(basketItem);

                _basketItemWriteRepository.Delete(basketItem);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                scope.Complete();
            }
        }

        public async Task RemoveCurrentBasketAsync(CancellationToken cancellationToken = default)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _basketWriteRepository.Delete(myBasket);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task UpdateItemQuantityAsync(Guid basketItemId, int quantity, CancellationToken cancellationToken = default)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                               .Include(b => b.Items)
                                    .ThenInclude(bi => bi.Product)
                               .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));

            if (basket.Items.Count > 0)
            {
                var basketItem = basket.Items.FirstOrDefault(bi => bi.Id == basketItemId);

                if (basketItem == null)
                {
                    throw new EntityNotFoundException(nameof(basketItem));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                basketItem.Quantity = Math.Clamp(quantity, 0, basketItem.Product.Stock);

                if (basketItem.Quantity == 0)
                {
                    basket.Items.Remove(basketItem);

                    _basketItemWriteRepository.Delete(basketItem);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                scope.Complete();
            }
        }


        public async Task UpdateItemQuantityAsync(Guid basketItemId, bool increment, CancellationToken cancellationToken = default)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                               .Include(b => b.Items)
                                    .ThenInclude(bi => bi.Product)
                               .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));

            if (basket.Items.Count > 0)
            {
                var basketItem = basket.Items.FirstOrDefault(bi => bi.Id == basketItemId);

                if (basketItem == null)
                {
                    throw new EntityNotFoundException(nameof(basketItem));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
                            basket.Items.Remove(basketItem);

                            _basketItemWriteRepository.Delete(basketItem);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                scope.Complete();
            }
        }

        public async Task ClearBasketAsync(CancellationToken cancellationToken = default)
        {
            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                               .Include(b => b.Items)
                               .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));

            if (basket.Items.Count > 0)
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                _basketItemWriteRepository.DeleteRange(basket.Items);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                scope.Complete();
            }
        }

        public async Task<bool> SyncBasketAsync(IEnumerable<GuestBasketDTO> guestBasket, CancellationToken cancellationToken = default)
        {
            if (!guestBasket.Any()) return false;

            var myBasket = await GetMyBasketAsync(cancellationToken);

            var basket = await _basketReadRepository.Table
                               .Include(b => b.Items)
                               .FirstOrDefaultAsync(b => b.Id == myBasket.Id, cancellationToken);

            if (basket == null) throw new EntityNotFoundException(nameof(basket));

            var products = _productReadRepository.Table.Where(p => guestBasket.Select(i => i.ProductId).Contains(p.Id)).AsEnumerable().Select(p => new { p.Id, p.Stock, Quantity = guestBasket.FirstOrDefault(i => i.ProductId == p.Id)!.Quantity });

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var product in products)
            {
                var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == product.Id);


                var quantity = Math.Min((existingItem?.Quantity ?? 0) + product.Quantity, product.Stock);

                if (existingItem != null)
                {
                    existingItem.Quantity = quantity;
                }
                else
                {

                    basket.Items.Add(new BasketItem
                    {
                        ProductId = product.Id,
                        Quantity = quantity,
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return true;
        }
    }
}
