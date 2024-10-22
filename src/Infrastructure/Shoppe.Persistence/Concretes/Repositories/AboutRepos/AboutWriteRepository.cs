using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.AboutRepos
{
    public class AboutWriteRepository : WriteRepository<About>, IAboutWriteRepository
    {
        public AboutWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
