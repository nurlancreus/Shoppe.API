using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.ReviewRepos
{
    public class ReviewReadRepository : ReadRepository<Review>, IReviewReadRepository
    {
        public ReviewReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
