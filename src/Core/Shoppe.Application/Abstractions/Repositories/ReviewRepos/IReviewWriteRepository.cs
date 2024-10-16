using Shoppe.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.ReviewRepos
{
    public interface IReviewWriteRepository : IWriteRepository<Review>
    {
    }
}
