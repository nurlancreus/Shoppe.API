using Shoppe.Application.Abstractions.Repositories.SlideRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.SlideRepos
{
    public class SlideWriteRepository : WriteRepository<Slide>, ISlideWriteRepository
    {
        public SlideWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
