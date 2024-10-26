using Shoppe.Application.Abstractions.Repositories.SlideRepos;
using Shoppe.Application.Abstractions.Repositories.SliderRepository;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.SlideRepos
{
    public class SlideReadRepository : ReadRepository<Slide>, ISlideReadRepository
    {
        public SlideReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
