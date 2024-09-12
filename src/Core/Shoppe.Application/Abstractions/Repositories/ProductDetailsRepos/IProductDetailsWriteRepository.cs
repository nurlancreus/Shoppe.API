using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.ProductDetailsRepos
{
    public interface IProductDetailsWriteRepository : IWriteRepository<ProductDetails>
    {
    }
}
