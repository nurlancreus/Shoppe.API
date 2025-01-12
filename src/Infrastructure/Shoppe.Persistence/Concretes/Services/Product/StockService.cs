using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class StockService : IStockService
    {
        private readonly IProductReadRepository _productReadRepository;

        public StockService(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public bool IsStockAvailable(Product product, int quantity)
        {
            return quantity <= product.Stock;
        }

        public void DeductStock(Product product, int quantity)
        {
            if (IsStockAvailable(product, quantity))
            {
                product.Stock -= quantity;
            }
            else throw new UpdateNotSucceedException("Product has no enough stock");
        }

        public void AddStock(Product product, int quantity)
        {
            product.Stock += quantity;
        }

        public async Task<bool> IsStockAvailableAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, cancellationToken, false);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            return IsStockAvailable(product, quantity);
        }

        public async Task DeduckStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            if (IsStockAvailable(product, quantity))
            {
                product.Stock -= quantity;
            }
            else throw new UpdateNotSucceedException("Product has no enough stock");
        }

        public async Task AddStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            product.Stock += quantity;

        }
    }
}
