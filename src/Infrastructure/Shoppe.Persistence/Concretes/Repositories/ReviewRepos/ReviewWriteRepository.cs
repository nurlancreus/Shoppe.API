using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.ReviewRepos
{
    public class ReviewWriteRepository : WriteRepository<Review>, IReviewWriteRepository
    {
        public ReviewWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
