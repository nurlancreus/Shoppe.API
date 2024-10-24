using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder
                .Property(s => s.Order)
                .HasColumnType("TINYINT") 
                .IsRequired();

            builder.ToTable(s => s.HasCheckConstraint("CK_Section_Order", "[Order] >= 0 AND [Order] <= 255"));

        }
    }

    public class AboutSectionConfiguration : IEntityTypeConfiguration<AboutSection>
    {
        public void Configure(EntityTypeBuilder<AboutSection> builder)
        {
            builder
                .HasMany(s => s.SectionImageFiles)
                .WithOne(i => i.Section)
                .HasForeignKey(i => i.SectionId)
                .IsRequired();
        }
    }
}
