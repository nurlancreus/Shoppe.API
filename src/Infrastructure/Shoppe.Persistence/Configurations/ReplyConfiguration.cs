using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder
                .HasDiscriminator(r => r.Type)
                .HasValue<BlogReply>("Blog");

            builder
                 .HasOne(r => r.Replier)
                 .WithMany(u => u.Replies)
                 .HasForeignKey(d => d.ReplierId)
                 .OnDelete(DeleteBehavior.NoAction);


            builder
                .HasOne(r => r.ParentReply)
                .WithMany(r => r.Replies)
                .HasForeignKey(r => r.ParentReplyId)
                .OnDelete(DeleteBehavior.NoAction);

            /*
             B. Handle Deletes in Your Application Logic
If you need to delete replies in a way that maintains the integrity of the relationships, consider handling the deletion in your application logic rather than relying solely on cascade deletes. For example, if you want to delete a ParentReply, you can manually delete all its child replies before deleting the parent.
             */

        }
        public class BlogReplyConfiguration : IEntityTypeConfiguration<BlogReply>
        {
            public void Configure(EntityTypeBuilder<BlogReply> builder)
            {
                builder
                    .HasOne(r => r.Blog)
                    .WithMany(b => b.Replies)
                    .HasForeignKey(r => r.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }

    }
}
