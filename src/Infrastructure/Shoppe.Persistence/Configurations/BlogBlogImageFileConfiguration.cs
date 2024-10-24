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
    public class BlogBlogImageFileConfiguration : IEntityTypeConfiguration<BlogBlogImage>
    {
        public void Configure(EntityTypeBuilder<BlogBlogImage> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.BlogImage)
                .WithMany(bi => bi.BlogMappings)
                .HasForeignKey(x => x.BlogImageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.BlogSection)
                .WithMany(bi => bi.BlogImageMappings)
                .HasForeignKey(x => x.BlogSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasIndex(x => new { x.BlogId, x.BlogImageId })
                .IsUnique();

            builder.HasIndex(x => new { x.BlogId, x.IsMain })
                   .IsUnique()
                   .HasFilter("[IsMain] = 1");
        }
    }
}
