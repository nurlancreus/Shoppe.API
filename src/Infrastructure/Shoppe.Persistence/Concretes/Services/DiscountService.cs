﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Discount;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using Shoppe.Domain.Flags;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountReadRepository _discountReadRepository;
        private readonly IDiscountWriteRepository _discountWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IPaginationService _paginationService;
        private readonly IUnitOfWork _unitOfWork;

        public DiscountService(
            IDiscountReadRepository discountReadRepository,
            IDiscountWriteRepository discountWriteRepository,
            IProductReadRepository productReadRepository,
            ICategoryReadRepository categoryReadRepository,
            IUnitOfWork unitOfWork,
            IPaginationService paginationService)
        {
            _discountReadRepository = discountReadRepository;
            _discountWriteRepository = discountWriteRepository;
            _productReadRepository = productReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
        }

        public async Task CreateAsync(CreateDiscountDTO createDiscountDTO, CancellationToken cancellationToken = default)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (string.IsNullOrEmpty(createDiscountDTO.Name))
                throw new ValidationException("Discount name cannot be empty");

            if (createDiscountDTO.DiscountPercentage is <= 0 or > 100)
                throw new ValidationException("Discount percentage must be between 0 and 100");

            if (createDiscountDTO.StartDate >= createDiscountDTO.EndDate)
                throw new ValidationException("End date must be later than start date");

            var discount = new Discount
            {
                Name = createDiscountDTO.Name,
                DiscountPercentage = createDiscountDTO.DiscountPercentage,
                StartDate = createDiscountDTO.StartDate,
                EndDate = createDiscountDTO.EndDate,
                IsActive = true,
            };

            await _discountWriteRepository.AddAsync(discount, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task AssignDiscountAsync(Guid entityId, Guid discountId, EntityType entityType, CancellationToken cancellationToken = default, bool update = false)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var discount = await _discountReadRepository.GetByIdAsync(discountId, cancellationToken);
            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            if (!CheckIfIsValid(discount))
                throw new InvalidOperationException("Discount is not valid for the current date");

            switch (entityType)
            {
                case EntityType.Product:
                    {
                        var product = await _productReadRepository.Table.Include(p => p.Discount).FirstOrDefaultAsync(p => p.Id == entityId, cancellationToken);
                        if (product == null)
                            throw new EntityNotFoundException(nameof(product));

                        var activeDiscount = product.Discount?.IsActive ?? false ? product.Discount : null;

                        if (activeDiscount != null)
                        {
                            if (!CheckIfIsValid(activeDiscount))
                            {
                                activeDiscount.IsActive = false;
                            }

                            else if (!update)
                            {
                                throw new InvalidOperationException("This product already has the assigned discount");
                            }
                        }

                        product.Discount = discount;
                        break;
                    }
                case EntityType.ProductCategory:
                    {
                        var category = await _categoryReadRepository.Table.OfType<ProductCategory>().Include(c => c.Discount).FirstOrDefaultAsync(c => c.Id == entityId, cancellationToken);
                        if (category == null)
                            throw new EntityNotFoundException(nameof(category));

                        var activeDiscount = category.Discount?.IsActive ?? false ? category.Discount : null;

                        if (activeDiscount != null)
                        {
                            if (!CheckIfIsValid(activeDiscount))
                            {
                                activeDiscount.IsActive = false;
                            }
                            else if (!update)
                            {
                                throw new InvalidOperationException("This category already has the assigned discount");
                            }
                        }

                        category.Discount = discount;
                        break;
                    }
                default:
                    throw new InvalidOperationException("Invalid entity type");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }


        public async Task AssignDiscountAsync(IDiscountable entity, Guid discountId, CancellationToken cancellationToken = default, bool update = false)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var discount = await _discountReadRepository.GetByIdAsync(discountId, cancellationToken);

            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            if (!CheckIfIsValid(discount))
                throw new InvalidOperationException("Discount is not valid for the current date");

            if (entity is Product product)
            {
                var activeDiscount = product.Discount?.IsActive ?? false ? product.Discount : null;

                if (activeDiscount != null)
                {

                    if (!CheckIfIsValid(activeDiscount))
                    {
                        activeDiscount.IsActive = false;
                    }

                    else if (!update)
                    {
                        throw new InvalidOperationException("This product already has the assigned discount");
                    }
                }


                product.Discount = discount;
            }
            else if (entity is ProductCategory category)
            {
                var activeDiscount = category.Discount?.IsActive ?? false ? category.Discount : null;

                if (activeDiscount != null)
                {

                    if (!CheckIfIsValid(activeDiscount))
                    {
                        activeDiscount.IsActive = false;
                    }

                    else if (!update)
                    {
                        throw new InvalidOperationException("This category already has the assigned discount");
                    }
                }


                category.Discount = discount;
            }
            else
            {
                throw new InvalidOperationException("Invalid entity type");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }


        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var discount = await _discountReadRepository.GetByIdAsync(id, cancellationToken);
            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            bool isDeleted = _discountWriteRepository.Delete(discount);

            if (!isDeleted) throw new DeleteNotSucceedException();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task<GetAllDiscountsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var discountsQuery = await _discountReadRepository.GetAllAsync(false);

            var paginatedResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, discountsQuery, cancellationToken);


            var discounts = await paginatedResult.PaginatedQuery
                .Include(d => d.Categories)
                .Include(d => d.Products)
                .Select(d => d.ToGetDiscountDTO(CheckIfIsValid)).ToListAsync(cancellationToken);

            return new GetAllDiscountsDTO
            {
                Discounts = discounts,
                Page = paginatedResult.Page,
                PageSize = paginatedResult.PageSize,
                TotalItems = paginatedResult.TotalItems,
                TotalPages = paginatedResult.TotalPages
            };
        }

        public async Task<GetDiscountDTO> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var discount = await _discountReadRepository.GetByIdAsync(id, cancellationToken);
            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            return discount.ToGetDiscountDTO(CheckIfIsValid);
        }

        public async Task UpdateAsync(UpdateDiscountDTO updateDiscountDTO, CancellationToken cancellationToken = default)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var discount = await _discountReadRepository.GetByIdAsync(updateDiscountDTO.Id, cancellationToken);

            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            if (updateDiscountDTO.Name is string name && name != discount.Name)
            {
                discount.Name = name;
            }

            if (updateDiscountDTO.Description is string description && description != discount.Description)
            {
                discount.Name = description;
            }

            if (updateDiscountDTO.DiscountPercentage is decimal percentage && percentage != discount.DiscountPercentage)
            {
                discount.DiscountPercentage = percentage;
            }

            if (updateDiscountDTO.StartDate is DateTime startDate && startDate != discount.StartDate)
            {
                discount.StartDate = startDate;
            }

            if (updateDiscountDTO.EndDate is DateTime endDate && endDate != discount.EndDate)
            {
                discount.EndDate = endDate;
            }

            if (updateDiscountDTO.IsActive is bool isActive)
            {

                discount.IsActive = isActive;
            }

            bool isUpdated = _discountWriteRepository.Update(discount);

            if (!isUpdated) throw new UpdateNotSucceedException();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task ToggleDiscountAsync(Guid discountId, CancellationToken cancellationToken = default)
        {
            var discount = await _discountReadRepository.GetByIdAsync(discountId, cancellationToken);

            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            if (!CheckIfIsValid(discount) && !discount.IsActive) throw new UpdateNotSucceedException("Discount is not valid anymore. If you want to activate it, you should update end date first.");

            else if (!CheckIfIsValid(discount) && discount.IsActive)
            {
                discount.IsActive = false;
            }
            else
            {
                discount.IsActive = !discount.IsActive;
            }

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            {
                throw new UpdateNotSucceedException();
            }
        }

        public static bool CheckIfIsValid(Discount? discount)
        {
            ArgumentNullException.ThrowIfNull(discount);

            return
                   discount.StartDate < discount.EndDate &&
                   DateTime.UtcNow >= discount.StartDate &&
                   DateTime.UtcNow < discount.EndDate;
        }
    }
}
