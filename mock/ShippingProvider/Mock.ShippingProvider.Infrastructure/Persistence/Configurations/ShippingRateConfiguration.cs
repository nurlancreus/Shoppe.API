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
    public class ShippingRateConfiguration : IEntityTypeConfiguration<ShippingRate>
    {
        public void Configure(EntityTypeBuilder<ShippingRate> builder)
        {
            builder.HasKey(s => s.Id);

            builder
                .Property(s => s.Method)
                .HasConversion<string>();

            builder
                .Property(s => s.Rate)
                .HasPrecision(12, 2);
        }
    }
}
