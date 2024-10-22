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
    public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialMediaLink>
    {
        public void Configure(EntityTypeBuilder<SocialMediaLink> builder)
        {
            builder.Property(l => l.SocialPlatform)
            .HasConversion<string>();
        }
    }
}
