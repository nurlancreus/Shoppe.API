﻿using Shoppe.Application.Abstractions.Repositories.SocialMediaRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.SocialMediaRepos
{
    public class SocialMediaLinkReadRepository : ReadRepository<SocialMediaLink>, ISocialMediaLinkReadRepository
    {
        public SocialMediaLinkReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
