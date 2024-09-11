using Shoppe.Application.Abstractions.Repositories.ProductImageFileRepos;
using Shoppe.Domain.Entities.Files;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.ProductImageFileRepos
{
    public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
