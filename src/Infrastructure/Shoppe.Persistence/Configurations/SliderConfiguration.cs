using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder
                .HasMany(s => s.Slides)
                .WithOne(sl => sl.Slider)
                .HasForeignKey(sl => sl.SliderId);

            builder
                .HasDiscriminator<string>("SliderType")
                .HasValue<HeroSlider>("Hero");
        }
    }

    public class SlideConfiguration : IEntityTypeConfiguration<Slide>
    {
        public void Configure(EntityTypeBuilder<Slide> builder)
        {
            builder
                .HasOne(s => s.SlideImageFile)
                .WithOne(si => si.Slide)
                .HasForeignKey<SlideImageFile>(si => si.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
