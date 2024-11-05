using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Discount;
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

        public async Task CreateAsync(CreateDiscountDTO createDiscountDTO, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (string.IsNullOrEmpty(createDiscountDTO.Name))
                throw new ValidationException("Discount name cannot be empty");

            if (createDiscountDTO.DiscountPercentage <= 0 || createDiscountDTO.DiscountPercentage > 100)
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
        
        public async Task AssignDiscountAsync(Guid entityId, Guid discountId, EntityType entityType, CancellationToken cancellationToken, bool update = false)
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
                        var product = await _productReadRepository.GetByIdAsync(entityId, cancellationToken);
                        if (product == null)
                            throw new EntityNotFoundException(nameof(product));

                        var activeDiscount = product.DiscountMappings.FirstOrDefault(dp => dp.Discount.IsActive);

                        if (activeDiscount != null)
                        {
                            if (!CheckIfIsValid(activeDiscount.Discount))
                            {
                               // activeDiscount.IsActive = false;
                                activeDiscount.Discount.IsActive = false;
                            }
                            else if (update)
                            {
                                product.DiscountMappings.Remove(activeDiscount);

                            }
                            else
                            {
                                throw new InvalidOperationException("This product already has the assigned discount");
                            }
                        }

                        product.DiscountMappings.Add(new DiscountProduct { DiscountId = discount.Id });
                        break;
                    }
                case EntityType.ProductCategory:
                    {
                        var category = await _categoryReadRepository.Table.OfType<ProductCategory>().FirstOrDefaultAsync(c => c.Id == entityId, cancellationToken);
                        if (category == null)
                            throw new EntityNotFoundException(nameof(category));

                        var activeDiscount = category.DiscountMappings.FirstOrDefault(dc => dc.Discount.IsActive);

                        if (activeDiscount != null)
                        {
                            if (!CheckIfIsValid(activeDiscount.Discount))
                            {
                                //activeDiscount.IsActive = false;
                                activeDiscount.Discount.IsActive = false;
                            }
                            else if (update)
                            {
                                category.DiscountMappings.Remove(activeDiscount);

                            }
                            else
                            {
                                throw new InvalidOperationException("This category already has the assigned discount");
                            }
                        }

                        category.DiscountMappings.Add(new DiscountCategory { DiscountId = discount.Id });
                        break;
                    }
                default:
                    throw new InvalidOperationException("Invalid entity type");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }


        public async Task AssignDiscountAsync(IDiscountable entity, Guid discountId, CancellationToken cancellationToken, bool update = false)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var discount = await _discountReadRepository.GetByIdAsync(discountId, cancellationToken);
            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            if (!CheckIfIsValid(discount))
                throw new InvalidOperationException("Discount is not valid for the current date");

            if (entity is Product product)
            {
                var activeDiscount = product.DiscountMappings.FirstOrDefault(dp => dp.Discount.IsActive);

                if (activeDiscount != null)
                {

                    if (!CheckIfIsValid(activeDiscount.Discount))
                    {
                       // activeDiscount.IsActive = false;
                        activeDiscount.Discount.IsActive = false;
                    }

                    else if (update)
                    {
                        product.DiscountMappings.Remove(activeDiscount);

                    }
                    else
                    {
                        throw new InvalidOperationException("This product already has the assigned discount");
                    }
                }


                // Add the new discount to the product
                product.DiscountMappings.Add(new DiscountProduct { DiscountId = discount.Id });
            }
            else if (entity is ProductCategory category)
            {
                var activeDiscount = category.DiscountMappings.FirstOrDefault(dc => dc.Discount.IsActive);

                if (activeDiscount != null)
                {

                    if (!CheckIfIsValid(activeDiscount.Discount))
                    {
                        //activeDiscount.IsActive = false;
                        activeDiscount.Discount.IsActive = false;
                    }

                    else if (update)
                    {
                        category.DiscountMappings.Remove(activeDiscount);

                    }

                    else
                    {
                        throw new InvalidOperationException("This category already has the assigned discount");
                    }
                }


                category.DiscountMappings.Add(new DiscountCategory { DiscountId = discount.Id });
            }
            else
            {
                throw new InvalidOperationException("Invalid entity type");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }


        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
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

        public async Task<GetAllDiscountsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var discountsQuery = await _discountReadRepository.GetAllAsync(false);

            var paginatedResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, discountsQuery, cancellationToken);


            var discounts = await paginatedResult.PaginatedQuery.Select(d => new GetDiscountDTO
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                IsActive = CheckIfIsValid(d),
                DiscountPercentage = d.DiscountPercentage,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                CreatedAt = d.CreatedAt,
            }).ToListAsync(cancellationToken: cancellationToken);

            return new GetAllDiscountsDTO
            {
                Discounts = discounts,
                Page = paginatedResult.Page,
                PageSize = paginatedResult.PageSize,
                TotalItems = paginatedResult.TotalItems,
                TotalPages = paginatedResult.TotalPages
            };
        }

        public async Task<GetDiscountDTO> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var discount = await _discountReadRepository.GetByIdAsync(id, cancellationToken);
            if (discount == null)
                throw new EntityNotFoundException(nameof(discount));

            return new GetDiscountDTO
            {
                Id = discount.Id,
                Name = discount.Name,
                Description = discount.Description,
                DiscountPercentage = discount.DiscountPercentage,
                IsActive = CheckIfIsValid(discount),
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                CreatedAt = discount.CreatedAt,
            };
        }

        public async Task UpdateAsync(UpdateDiscountDTO updateDiscountDTO, CancellationToken cancellationToken)
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

        private static bool CheckIfIsValid(Discount? discount)
        {
            ArgumentNullException.ThrowIfNull(discount);

            return discount.IsActive &&
                   discount.StartDate < discount.EndDate &&
                   DateTime.UtcNow >= discount.StartDate &&
                   DateTime.UtcNow < discount.EndDate;
        }
    }
}
