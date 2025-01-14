using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Infrastructure.Persistence.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder
                .HasOne(s => s.OriginAddress)
                .WithMany(a => a.ShipmentsOrigin)
                .HasForeignKey(s => s.OriginAddressId)
                .IsRequired(false);

            builder
               .HasOne(s => s.DestinationAddress)
               .WithMany(a => a.ShipmentsDestination)
               .HasForeignKey(s => s.DestinationAddressId);

            builder
                .HasOne(s => s.ApiClient)
                .WithMany(c => c.Shipments)
                .HasForeignKey(s => s.ApiClientId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
