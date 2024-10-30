using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ProductImageFileConfiguration : IEntityTypeConfiguration<ProductImageFile>
    {
        public void Configure(EntityTypeBuilder<ProductImageFile> builder)
        {
            // Ensure only one image per product can be IsMain
            builder.HasIndex(pif => new { pif.ProductId, pif.IsMain })
                .IsUnique()
                .HasFilter("[ProductId] IS NOT NULL AND [IsMain] = 1");
        }
    }
}
