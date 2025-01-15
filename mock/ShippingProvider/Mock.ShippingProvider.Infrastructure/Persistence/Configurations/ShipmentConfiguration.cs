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
            builder.HasKey(s => s.Id);

            builder
                .HasIndex(s => s.TrackingNumber)
                .IsUnique();

            builder
                .Property(s => s.Status)
                .HasConversion<string>();

            builder
                .HasOne(s => s.Rate)
                .WithOne(r => r.Shipment)
                .HasForeignKey<ShippingRate>(r => r.ShipmentId);

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

            builder.ToTable(bi => bi.HasCheckConstraint("CK_Shipment_EstimatedDate", "[EstimatedDate] > getdate()"));
        }
    }
}
