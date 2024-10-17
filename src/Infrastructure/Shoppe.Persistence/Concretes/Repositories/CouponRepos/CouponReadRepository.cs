using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.CouponRepos
{
    public class CouponReadRepository : ReadRepository<Coupon>, ICouponReadRepository
    {
        public CouponReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
