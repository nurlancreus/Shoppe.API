using Shoppe.Application.DTOs.Category;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO, CancellationToken cancellationToken);
        Task<GetCategoryDTO> GetCategoryAsync (Guid id, CancellationToken cancellationToken);
        Task<GetAllCategoriesDTO> GetAllCategoriesAsync(int page, int pageSize, CategoryType? type, CancellationToken cancellationToken);
        Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken);
    }
}
