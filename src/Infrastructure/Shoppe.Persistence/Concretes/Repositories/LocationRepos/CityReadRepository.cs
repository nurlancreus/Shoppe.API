﻿using Shoppe.Application.Abstractions.Repositories.LocationRepos;
using Shoppe.Domain.Entities;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.LocationRepos
{ 
    public class CityReadRepository : ReadRepository<City>, ICityReadRepository
    {
        public CityReadRepository(ShoppeDbContext context) : base(context)
        {

        }
    }
}
