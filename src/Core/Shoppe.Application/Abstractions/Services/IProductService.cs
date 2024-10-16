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
        Task DeleteProductAsync(string id, CancellationToken cancellationToken);
        Task<GetProductDTO> GetProductAsync(string id, CancellationToken cancellationToken);
        Task<GetAllProductsDTO> GetAllProductsAsync(ProductFilterParamsDTO filtersDTO, CancellationToken cancellationToken);
        Task<List<GetReviewDTO>> GetReviewsByProductAsync(string productId, CancellationToken cancellationToken);
        Task ChangeMainImageAsync(string productId, string newMainImageId, CancellationToken cancellationToken);
        Task RemoveImageAsync(string productId, string newMainImageId, CancellationToken cancellationToken);
    }
}
