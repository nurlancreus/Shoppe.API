using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder
              .HasOne(pd => pd.Dimension)
              .WithOne(d => d.ProductDetails)
              .HasForeignKey<ProductDimension>(d => d.Id);

            builder
                .Property(pd => pd.Material)
                .HasConversion<string>();

            builder
                    .Property(pd => pd.Colors)
                    .HasConversion(
                    v => string.Join(',', v), // Convert List<Color> to a comma-separated string
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => (Color)Enum.Parse(typeof(Color), c)).ToList() // Convert back to List<Color>
            );
        }
    }
}
