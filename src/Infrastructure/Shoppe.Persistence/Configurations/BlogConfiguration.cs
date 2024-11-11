using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Entities.Tags;

namespace Shoppe.Persistence.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            // Set up the primary key and other properties
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired(); // Ensure Title is required

            // Set up the relationships
            builder.HasOne(b => b.BlogCoverImageFile)
                .WithMany(i => i.Blogs) // Assuming BlogImageFile has a collection of Blogs
                .HasForeignKey(b => b.BlogCoverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Author)
                .WithMany(u => u.Blogs) // Assuming ApplicationUser has a collection of Blogs
                .HasForeignKey(b => b.AuthorId);

            builder.HasMany(b => b.Replies)
                .WithOne(br => br.Blog) // Assuming BlogReply has a navigation property to Blog
                .HasForeignKey(br => br.BlogId);

            builder.HasMany(b => b.Reactions)
                .WithOne(br => br.Blog) // Assuming BlogReaction has a navigation property to Blog
                .HasForeignKey(br => br.BlogId);
        }
    }
}
