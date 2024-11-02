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
    public class UserProfilePictureFileConfiguration : IEntityTypeConfiguration<UserProfileImageFile>
    {
        public void Configure(EntityTypeBuilder<UserProfileImageFile> builder)
        {
            builder
                .HasIndex(pp => new { pp.UserId, pp.IsMain})
                .IsUnique()
                .HasFilter("[UserId] IS NOT NULL AND [IsMain] = 1");
        }
    }
}
