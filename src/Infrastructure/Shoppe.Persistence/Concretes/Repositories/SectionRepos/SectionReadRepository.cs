using Shoppe.Application.Abstractions.Repositories.SectionRepos;
using Shoppe.Domain.Entities.Sections;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.SectionRepos
{
    public class SectionReadRepository : ReadRepository<Section>, ISectionReadRepository
    {
        public SectionReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
