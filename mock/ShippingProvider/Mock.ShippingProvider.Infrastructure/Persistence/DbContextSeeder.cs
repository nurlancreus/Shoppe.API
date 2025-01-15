using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Interfaces.Services;
using Mock.ShippingProvider.Domain.Entities;

namespace Mock.ShippingProvider.Infrastructure.Persistence
{
    public static class DbContextSeeder
    {
        public static ModelBuilder SeedClient(this ModelBuilder builder)
        {
            var client = ApiClient.Create("My Company");

            client.Id = Guid.NewGuid();
            client.CreatedAt = DateTime.UtcNow;

            var address = Address.Create("Azerbaijan", "Baku", null, "AZ1000", "Ashiq Molla", 40.3755885, 49.8328009);

            address.Id = Guid.NewGuid();
            address.CreatedAt = DateTime.UtcNow;
            address.ClientId = client.Id;

            builder.Entity<ApiClient>().HasData(client);
            builder.Entity<Address>().HasData(address);

            return builder;
        }
    }
}
