using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;

namespace Shoppe.Persistence.Configurations
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            // Index on UserId for efficient querying by user (optional)
            builder.HasIndex(r => r.UserId);

            // Define the discriminator for TPH (Table Per Hierarchy)
            builder.HasDiscriminator(r => r.EntityType)
                .HasValue<ReplyReaction>("Reply")
                .HasValue<BlogReaction>("Blog");


        }
    }

    public class BlogReactionConfiguration : IEntityTypeConfiguration<BlogReaction>
    {
        public void Configure(EntityTypeBuilder<BlogReaction> builder)
        {
            builder.HasIndex(r => new { r.UserId, r.BlogId })
                .IsUnique();

            builder.Property(r => r.BlogReactionType)
                .HasConversion<string>();

            builder
                .HasOne(r => r.Blog) 
                .WithMany(b => b.Reactions) 
                .HasForeignKey(r => r.BlogId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }

    public class ReplyReactionConfiguration : IEntityTypeConfiguration<ReplyReaction>
    {
        public void Configure(EntityTypeBuilder<ReplyReaction> builder)
        {
            builder.HasIndex(r => new { r.UserId, r.ReplyId })
                .IsUnique();

            builder.Property(r => r.ReplyReactionType)
                .HasConversion<string>();

            builder
                .HasOne(r => r.Reply)
                .WithMany(reply => reply.Reactions)
                .HasForeignKey(r => r.ReplyId)
                .OnDelete(DeleteBehavior.NoAction); 
        }

    }
}
