using Shoppe.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.CategoryRepos
{
    public interface ICategoryReadRepository : IReadRepository<Category>
    {
    }
}
