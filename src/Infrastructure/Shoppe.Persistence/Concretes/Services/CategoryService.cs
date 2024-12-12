using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities.Categories;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;

        public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork, IPaginationService paginationService)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
        }

        public async Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var existedCategory = await _categoryReadRepository.GetAsync(c => c.Name == createCategoryDTO.Name, cancellationToken, false);

            if (existedCategory != null)
            {
                throw new AddNotSucceedException("Category already exists.");
            }

            Category? category = null;


            if (createCategoryDTO.Type == CategoryType.Product)
            {
                category = new ProductCategory
                {
                    Name = createCategoryDTO.Name,
                    Description = createCategoryDTO.Description,
                };
            }
            else if (createCategoryDTO.Type == CategoryType.Blog)
            {
                category = new BlogCategory
                {
                    Name = createCategoryDTO.Name,
                    Description = createCategoryDTO.Description,
                };
            }

            await _categoryWriteRepository.AddAsync(category!, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var category = await _categoryReadRepository.GetByIdAsync(id, cancellationToken);

            if (category == null)
            {
                throw new EntityNotFoundException(nameof(category));
            }

            bool isDeleted = _categoryWriteRepository.Delete(category);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }

        public async Task<GetAllCategoriesDTO> GetAllCategoriesAsync(int page, int pageSize, CategoryType? type, CancellationToken cancellationToken)
        {
            IQueryable<Category>? query = null;

            if (type == CategoryType.Product)
            {
                query = _categoryReadRepository.Table.OfType<ProductCategory>().Include(c => c.Discount).AsNoTracking();
            }
            else if (type == CategoryType.Blog)
            {
                query = _categoryReadRepository.Table.OfType<BlogCategory>().AsNoTracking();
            }

            else
            {
                query = await _categoryReadRepository.GetAllAsync(false);
            }

            if (query == null)
            {
                throw new InvalidOperationException("Query not found, operation invalid");
            }

            var (totalItems, _pageSize, _page, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(page, pageSize, query, cancellationToken);

            var categories = await paginatedQuery.Select(c => c.ToGetCategoryDTO()).ToListAsync(cancellationToken);

            return new GetAllCategoriesDTO
            {
                Categories = categories,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<GetCategoryDTO> GetCategoryAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryReadRepository.GetByIdAsync(id, cancellationToken, false);

            if (category == null)
            {
                throw new EntityNotFoundException(nameof(category));
            }

            return category.ToGetCategoryDTO();
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var category = await _categoryReadRepository.GetByIdAsync(updateCategoryDTO.Id, cancellationToken);

            if (category == null)
            {
                throw new EntityNotFoundException(nameof(category));
            }

            if (!string.IsNullOrWhiteSpace(updateCategoryDTO.Name) && category.Name.ToLower() != updateCategoryDTO.Name.ToLower())
            {
                category.Name = updateCategoryDTO.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateCategoryDTO.Description) && category.Description?.ToLower() != updateCategoryDTO.Description.ToLower())
            {
                category.Description = updateCategoryDTO.Description;
            }


            var isUpdated = _categoryWriteRepository.Update(category);

            if (!isUpdated)
            {
                throw new UpdateNotSucceedException();
            }


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

    }
}
