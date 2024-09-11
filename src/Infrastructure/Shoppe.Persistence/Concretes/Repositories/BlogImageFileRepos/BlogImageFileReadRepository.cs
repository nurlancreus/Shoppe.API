using Shoppe.Application.Abstractions.Repositories.BlogImageFileRepos;
using Shoppe.Domain.Entities.Files;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.BlogImageFileRepos
{
    public class BlogImageFileReadRepository : ReadRepository<BlogImageFile>, IBlogImageFileReadRepository
    {
        public BlogImageFileReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
