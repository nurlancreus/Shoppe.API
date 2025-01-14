using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain.Entities.Base;
using Mock.ShippingProvider.Infrastructure.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// add-migration init -OutputDir ./Persistence/Migrations

namespace Mock.ShippingProvider.Infrastructure.Persistence
{
    public class ShippingProviderDbContext(DbContextOptions<ShippingProviderDbContext> options) : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApiClientSeeder.SeedClient(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ShipmentConfiguration))!);

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateDateTimesWhileSavingInterceptor();

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<ApiClient> ApiClients { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ShippingRate> ShippingRates { get; set; }


        private void UpdateDateTimesWhileSavingInterceptor()
        {
            var changedEntries = ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (var entry in changedEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
