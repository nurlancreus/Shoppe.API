using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.TagRepos
{
    public class TagReadRepository : ReadRepository<Tag>, ITagReadRepository
    {
        public TagReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
