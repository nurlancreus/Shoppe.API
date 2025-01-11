using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Order)
                .WithOne(x => x.Shipment)
                .HasForeignKey<Shipment>(x => x.OrderId);

            builder
                .Property(x=> x.Provider)
                .HasConversion<string>();

            builder
                .Property(x => x.Status)
                .HasConversion<string>();

            builder
                .Property(x => x.Method)
                .HasConversion<string>();
        }
    }
}
