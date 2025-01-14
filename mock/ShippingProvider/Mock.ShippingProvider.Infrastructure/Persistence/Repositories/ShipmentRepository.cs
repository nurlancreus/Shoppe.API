using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Domain.Entities;

namespace Mock.ShippingProvider.Infrastructure.Persistence.Repositories
{
    public class ShipmentRepository(ShippingProviderDbContext dbContext, DbSet<Shipment> dbSet) : Repository<Shipment>(dbContext, dbSet), IShipmentRepository
    {
    }
}
