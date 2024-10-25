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
            builder.HasKey(x => new {x.BlogSectionId, x.BlogImageId});

            builder
                .HasOne(x => x.BlogImage)
                .WithMany(bi => bi.BlogMappings)
                .HasForeignKey(x => x.BlogImageId)
                //.IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.BlogSection)
                .WithMany(bi => bi.BlogImageMappings)
                .HasForeignKey(x => x.BlogSectionId)
                //.IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
