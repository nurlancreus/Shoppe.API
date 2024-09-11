using Shoppe.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(string name, CancellationToken cancellationToken);
        Task<GetCategoryDTO> GetCategoryAsync (string id, CancellationToken cancellationToken);
        Task<GetAllCategoriesDTO> GetAllCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(string id, CancellationToken cancellationToken);
    }
}
