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
        }
    }

    public class DiscountProductConfiguration : IEntityTypeConfiguration<DiscountProduct>
    {
        public void Configure(EntityTypeBuilder<DiscountProduct> builder)
        {
            builder.HasKey(dp => dp.Id);

            builder
                .HasOne(dp => dp.Product)
                .WithMany(p => p.DiscountMappings)
                .HasForeignKey(dp => dp.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(dp => dp.Discount)
                .WithMany(d => d.ProductMappings)
                .HasForeignKey(dp => dp.DiscountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(dp => new { dp.ProductId, dp.DiscountId })
                  .IsUnique();

            builder.HasIndex(dp => new { dp.ProductId, dp.IsActive })
                   .IsUnique()
                   .HasFilter("[IsActive] = 1");
        }
    }

    public class DiscountCategoryConfiguration : IEntityTypeConfiguration<DiscountCategory>
    {
        public void Configure(EntityTypeBuilder<DiscountCategory> builder)
        {
            builder.HasKey(dp => dp.Id);

            builder
                .HasOne(dp => dp.Category)
                .WithMany(p => p.DiscountMappings)
                .HasForeignKey(dp => dp.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(dp => dp.Discount)
                .WithMany(d => d.CategoryMappings)
                .HasForeignKey(dp => dp.DiscountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(dp => new { dp.CategoryId, dp.DiscountId })
                  .IsUnique();

            builder.HasIndex(dp => new { dp.CategoryId, dp.IsActive })
                   .IsUnique()
                   .HasFilter("[IsActive] = 1");
        }
    }
}
