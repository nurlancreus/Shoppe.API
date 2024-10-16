using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
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
            builder
               .HasOne(p => p.ProductDetails)
               .WithOne(pd => pd.Product)
               .HasForeignKey<ProductDetails>(p => p.Id);

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
