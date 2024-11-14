using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Domain.Entities.Reviews;

namespace Shoppe.Persistence.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .HasDiscriminator(c => c.Type)
                .HasValue<BlogTag>("Blog");
        }
    }

    public class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {

            builder.HasData(
                new BlogTag
                { Id = Guid.NewGuid(), Name = "Fashion", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogTag
                { Id = Guid.NewGuid(), Name = "Jewelry Care", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogTag
                { Id = Guid.NewGuid(), Name = "Gemstones", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogTag
                { Id = Guid.NewGuid(), Name = "DIY Jewelry", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogTag
                { Id = Guid.NewGuid(), Name = "Trends", Type = "Blog", CreatedAt = DateTime.UtcNow }
            );
        }
    }
}
