using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews);

            builder
                  .HasOne(r => r.Reviewer)
                  .WithMany(u => u.Reviews)
                  .HasForeignKey(d => d.ApplicationUserId)
                  .IsRequired(false);
        }
    }
}
