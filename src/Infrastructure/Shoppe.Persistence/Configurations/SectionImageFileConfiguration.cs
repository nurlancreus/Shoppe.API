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
    public class SectionImageFileConfiguration : IEntityTypeConfiguration<AboutSectionImageFile>
    {
        public void Configure(EntityTypeBuilder<AboutSectionImageFile> builder)
        {
            builder.HasIndex(sif => new { sif.SectionId, sif.IsMain })
                .IsUnique()
                .HasFilter("[IsMain] = 1");
        }
    }
}
