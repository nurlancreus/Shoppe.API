using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;
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

            var aboutId = new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb");  // Use the same AboutId from the About entity seed

            builder.HasData
            (
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.Facebook,
                    URL = "https://facebook.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                },
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.X,
                    URL = "https://x.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt= DateTime.UtcNow,
                },
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.Instagram,
                    URL = "https://instagram.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                },
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.Youtube,
                    URL = "https://youtube.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                }
            );
        }
    }
}
