using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CreateCategoryAsync(string name, CancellationToken cancellationToken)
        {
            var existedCategory = await _categoryReadRepository.GetAsync(c => c.Name == name, cancellationToken, false);

            if (existedCategory != null)
            {
                throw new Exception("Category already exists.");
            }

            var category = new Category()
            {
                Name = name,
            };

            await _categoryWriteRepository.AddAsync(category, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategoryAsync(string id, CancellationToken cancellationToken)
        {
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
        }

        public async Task<GetAllCategoriesDTO> GetAllCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var categoriesQuery = await _categoryReadRepository.GetAllAsync(false);

            var (totalItems, _pageSize, _page, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(page, pageSize, categoriesQuery);

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

        public async Task<GetCategoryDTO> GetCategoryAsync(string id, CancellationToken cancellationToken)
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
            var category = await _categoryReadRepository.GetByIdAsync(updateCategoryDTO.Id, cancellationToken);

            if (category == null)
            {
                throw new EntityNotFoundException(nameof(category));

            }

            category.Name = updateCategoryDTO.Name;

            var isUpdated = _categoryWriteRepository.Update(category);

            if (!isUpdated)
            {
                throw new UpdateNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
