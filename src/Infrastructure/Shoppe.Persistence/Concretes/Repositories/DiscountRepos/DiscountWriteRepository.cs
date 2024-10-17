using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.DiscountRepos
{
    public class DiscountWriteRepository : WriteRepository<Discount>, IDiscountWriteRepository
    {
        public DiscountWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
