using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Interfaces.Services;
using Mock.ShippingProvider.Domain.Entities;

namespace Mock.ShippingProvider.Infrastructure.Persistence
{
    public static class ApiClientSeeder
    {
        public static void SeedClient(ModelBuilder builder)
        {
            var client = ApiClient.Create("My Company");

            client.Id = Guid.NewGuid();
            var address = Address.Create("Azerbaijan", "Baku", null, "AZ1000", "Ashiq Molla", 40.3755885, 49.8328009);

            address.Id = Guid.NewGuid();

            client.AddressId = address.Id;

            builder.Entity<ApiClient>().HasData(client);
               
        }
    }
}
