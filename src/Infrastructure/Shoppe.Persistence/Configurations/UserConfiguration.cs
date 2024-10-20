using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasMany(u => u.ProfilePictureFiles)
                .WithOne(pp => pp.User)
                .HasForeignKey(pp => pp.UserId)
                .OnDelete(DeleteBehavior.NoAction);
           
        }
    }
}
