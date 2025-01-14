using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Infrastructure.Persistence.Repositories
{
    public class ApiClientRepository(ShippingProviderDbContext dbContext, DbSet<ApiClient> dbSet) : Repository<ApiClient>(dbContext, dbSet), IApiClientRepository
    {
    }
}
