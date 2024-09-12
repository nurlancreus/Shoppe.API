using Shoppe.Application.Abstractions.Repositories.ProductDetailsRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.ProductDetailsRepos
{
    public class ProductDetailsWriteRepository : WriteRepository<ProductDetails>, IProductDetailsWriteRepository
    {
        public ProductDetailsWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
