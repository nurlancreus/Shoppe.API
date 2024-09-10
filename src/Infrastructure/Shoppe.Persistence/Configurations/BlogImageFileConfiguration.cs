using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class BlogImageFileConfiguration : IEntityTypeConfiguration<BlogImageFile>
    {
        public void Configure(EntityTypeBuilder<BlogImageFile> builder)
        {
            builder.HasMany(bi => bi.Blogs)
            .WithMany(b => b.BlogImageFiles)
            .UsingEntity<BlogBlogImage>(
                j => j
                    .HasOne(bb => bb.Blog)
                    .WithMany()
                    .HasForeignKey(bb => bb.BlogId),
                j => j
                    .HasOne(bb => bb.BlogImage)
                    .WithMany()
                    .HasForeignKey(bb => bb.BlogImageId),
                j =>
                {
                    j.HasKey(bb => new { bb.BlogId, bb.BlogImageId });
                    //j.Property(bb => bb.IsMain).IsRequired();
                });

            // Unique constraint for IsMain column per Blog
            builder.HasIndex(bi => new { bi.Id, bi.IsMain })
                .IsUnique()
                .HasFilter("[IsMain] = 1");

        }
    }
}
