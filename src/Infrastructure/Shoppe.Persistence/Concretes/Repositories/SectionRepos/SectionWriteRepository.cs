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
    public class SectionWriteRepository : WriteRepository<Section>, ISectionWriteRepository
    {
        public SectionWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
