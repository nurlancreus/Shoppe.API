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
        Task CreateProductAsync(CreateProductDTO createProductDTO, CancellationToken cancellationToken);
        Task UpdateProductAsync(UpdateProductDTO updateProductDTO, CancellationToken cancellationToken);
        Task DeleteProductAsync(Guid id, CancellationToken cancellationToken);
        Task<GetProductDTO> GetProductAsync(Guid id, CancellationToken cancellationToken);
        Task<GetAllProductsDTO> GetAllProductsAsync(ProductFilterParamsDTO filtersDTO, CancellationToken cancellationToken);
        Task ChangeMainImageAsync(Guid productId, Guid newMainImageId, CancellationToken cancellationToken);
        Task RemoveImageAsync(Guid productId, Guid newMainImageId, CancellationToken cancellationToken);
    }
}
