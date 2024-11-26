using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using System.Collections.Generic;

namespace Shoppe.Persistence.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(d => d.DiscountPercentage)
                   .HasColumnType("decimal(5, 2)")
                   .IsRequired();

            builder.HasMany(d => d.Categories)
                   .WithOne(c => c.Discount)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(d => d.Products)
                   .WithOne(p => p.Discount)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable(bi => bi.HasCheckConstraint("CK_Discount_DiscountPercentage", "[DiscountPercentage] > 0 AND [DiscountPercentage] <= 100"));
        }
    }

}
