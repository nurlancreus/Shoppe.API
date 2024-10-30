using Shoppe.Application.Abstractions.Repositories.SocialMediaRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.SocialMediaRepos
{
    public class SocialMediaLinkWriteRepository : WriteRepository<SocialMediaLink>, ISocialMediaLinkWriteRepository
    {
        public SocialMediaLinkWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
