using Shoppe.Application.DTOs.Product;
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
        Task<GetAllProductsDTO> GetAllProductsAsync(int page, int size, CancellationToken cancellationToken);
    }
}
