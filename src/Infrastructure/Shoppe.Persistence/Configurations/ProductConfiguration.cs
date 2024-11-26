using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Value Converter for Materials
            var materialsConverter = new ValueConverter<ICollection<Material>, string>(
                v => string.Join(',', v), // Convert to a single string for storage
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(c => (Material)Enum.Parse(typeof(Material), c))
                       .ToList() // Convert back to a List<Material>
            );

            // Value Comparer for Materials
            var materialsComparer = new ValueComparer<ICollection<Material>>(
                (c1, c2) => c1.SequenceEqual(c2), // Compare collections for equality
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Compute hash code
                c => c.ToList() // Create a copy of the collection
            );

            // Value Converter for Colors
            var colorsConverter = new ValueConverter<ICollection<Color>, string>(
                v => string.Join(',', v), // Convert to a single string for storage
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(c => (Color)Enum.Parse(typeof(Color), c))
                       .ToList() // Convert back to a List<Color>
            );

            // Value Comparer for Colors
            var colorsComparer = new ValueComparer<ICollection<Color>>(
                (c1, c2) => c1.SequenceEqual(c2), // Compare collections for equality
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Compute hash code
                c => c.ToList() // Create a copy of the collection
            );

            builder.OwnsOne(p => p.ProductDetails, add =>
            {
                add.Property(pd => pd.Weight).HasColumnName(nameof(ProductDetails.Weight));
                add.Property(pd => pd.Width).HasColumnName(nameof(ProductDetails.Width));
                add.Property(pd => pd.Height).HasColumnName(nameof(ProductDetails.Height));
                add.Property(pd => pd.Materials).HasColumnName(nameof(ProductDetails.Materials));
                add.Property(pd => pd.Colors).HasColumnName(nameof(ProductDetails.Colors));

                // Apply the converter and comparer to the Materials property
                add
                    .Property(pd => pd.Materials)
                    .HasConversion(materialsConverter)
                    .Metadata.SetValueComparer(materialsComparer);

                // Apply the converter and comparer to the Colors property
                add
                    .Property(pd => pd.Colors)
                    .HasConversion(colorsConverter)
                    .Metadata.SetValueComparer(colorsComparer);

                add.ToTable("ProductDetails");
            });

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId);

            builder.HasMany(p => p.ProductImageFiles)
               .WithOne(pif => pif.Product)
               .HasForeignKey(pif => pif.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
