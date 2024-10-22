using Shoppe.Application.Abstractions.Repositories.SliderRepository;
using Shoppe.Domain.Entities.Sliders;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.SliderRepos
{
    public class SliderWriteRepository : WriteRepository<Slider>, ISliderWriteRepository
    {
        public SliderWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
