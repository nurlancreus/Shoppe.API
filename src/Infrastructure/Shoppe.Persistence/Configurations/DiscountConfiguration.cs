using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
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

            builder
                .HasMany(d => d.Products)
                .WithMany(p => p.Discounts)
                .UsingEntity<Dictionary<string, object>>(
                    "DiscountProduct", // Table name for the implicit join table
                    j =>
                    {
                        return j.HasOne<Product>()
                             .WithMany()
                             .HasForeignKey("ProductId")
                             .OnDelete(DeleteBehavior.Cascade);
                    },
                    j =>
                    {
                        return j.HasOne<Discount>()
                            .WithMany()
                            .HasForeignKey("DiscountId")
                            .OnDelete(DeleteBehavior.Cascade);
                    });
        }
    }
}
