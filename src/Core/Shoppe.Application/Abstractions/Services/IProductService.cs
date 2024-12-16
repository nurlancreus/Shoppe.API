using Shoppe.Application.DTOs.Product;
using Shoppe.Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task CreateAsync(CreateProductDTO createProductDTO, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateProductDTO updateProductDTO, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetProductDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetAllProductsDTO> GetAllAsync(ProductFilterParamsDTO filtersDTO, CancellationToken cancellationToken = default);
        Task<List<GetProductDTO>> GetProductsById(IEnumerable<Guid> productsIds, CancellationToken cancellationToken = default);
        Task ChangeMainImageAsync(Guid productId, Guid newMainImageId, CancellationToken cancellationToken = default);
        Task RemoveImageAsync(Guid productId, Guid newMainImageId, CancellationToken cancellationToken = default);
    }
}
