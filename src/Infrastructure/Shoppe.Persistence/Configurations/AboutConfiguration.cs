using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Application.Constants;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class AboutConfiguration : IEntityTypeConfiguration<About>
    {
        public void Configure(EntityTypeBuilder<About> builder)
        {
            builder
                .HasKey(a => a.Id);  // Ensure Id is the primary key

            builder.HasIndex(a => a.Id).IsUnique();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(AboutConst.MaxNameLength);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(AboutConst.MaxDescLength);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(AboutConst.MaxEmailLength)
                .HasAnnotation("RegexPattern", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");  // Email format validation

            builder.Property(a => a.Phone)
                .IsRequired()
                .HasMaxLength(AboutConst.MaxPhoneLength)
                .HasAnnotation("RegexPattern", @"^\+?\d{1,3}?[-.●]?\(?\d{1,4}?\)?[-.●]?\d{1,4}[-.●]?\d{1,9}$");  // Phone format validation

            //builder.HasData(new About
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Shoppe",
            //    Description = "Who we are and why we do what we do!",
            //    Email = "contact@shoppe.com",  
            //    Phone = "123-456-7890",
            //    CreatedAt = DateTime.UtcNow,
            //});



            builder
                .HasMany(a => a.SocialMediaLinks)
                .WithOne();
            //.HasForeignKey("ShopId")
            //.IsRequired();

            builder
                .HasMany(a => a.SocialMediaLinks)
                .WithOne();
            //.HasForeignKey("ShopId")
            //.IsRequired();

        }
    }
}
