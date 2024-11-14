using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                 .HasDiscriminator(c => c.Type)
                 .HasValue<ProductCategory>("Product");

            builder
                .HasDiscriminator(c => c.Type)
                .HasValue<BlogCategory>("Blog");

        }
    }

    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            // Seed data for ProductCategory
            builder.HasData(
                new ProductCategory { Id = Guid.NewGuid(), Name = "Necklaces", Description = "Elegant and modern necklaces", Type = "Product", CreatedAt = DateTime.UtcNow },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Earrings", Description = "Stylish earrings for all occasions", Type = "Product", CreatedAt = DateTime.UtcNow },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Bracelets", Description = "Beautiful bracelets in various styles", Type = "Product", CreatedAt = DateTime.UtcNow },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Rings", Description = "Rings for engagement, fashion, and more", Type = "Product", CreatedAt = DateTime.UtcNow },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Brooches", Description = "Unique brooches to complement any outfit", Type = "Product", CreatedAt = DateTime.UtcNow }
            );

        }
    }

    public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
    {
        public void Configure(EntityTypeBuilder<BlogCategory> builder)
        {
            // Seed data for BlogCategory
            builder.HasData(
                new BlogCategory { Id = Guid.NewGuid(), Name = "Jewelry Care", Description = "Tips on how to take care of your jewelry", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "Latest Trends", Description = "Updates on the latest jewelry trends", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "Gemstone Guide", Description = "Learn about different gemstones and their meanings", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "Gift Ideas", Description = "Jewelry gift ideas for various occasions", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "DIY Jewelry", Description = "Guides and inspiration for making your own jewelry", Type = "Blog", CreatedAt = DateTime.UtcNow }
            );

        }
    }
}
