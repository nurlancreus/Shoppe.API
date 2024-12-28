using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasDiscriminator(a => a.AddressType)
                .HasValue<BillingAddress>("Billing")
                .HasValue<ShippingAddress>("Shipping");

            builder.
               HasIndex(a => new
               { a.AddressType, a.FirstName, a.LastName, a.Email, a.Country, a.City, a.PostalCode, a.StreetAddress })
               .IsUnique();
        }
    }

    public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {

            builder
                .HasOne(a => a.Account)
                .WithOne(u => u.ShippingAddress)
                .HasForeignKey<ShippingAddress>(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(a => a.UserId)
                .HasColumnName("ShippingAddress_UserId");

            builder
                .HasIndex(a => new { a.UserId })
                .IsUnique()
                .HasFilter("[ShippingAddress_UserId] IS NOT NULL");

        }
    }

    public class BillingAddressConfiguration : IEntityTypeConfiguration<BillingAddress>
    {
        public void Configure(EntityTypeBuilder<BillingAddress> builder)
        {

            builder
                .HasOne(a => a.Account)
                .WithOne(u => u.BillingAddress)
                .HasForeignKey<BillingAddress>(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(a => a.UserId)
                .HasColumnName("BillingAddress_UserId");

            builder
                .HasIndex(a => new { a.UserId })
                .IsUnique()
                .HasFilter("[BillingAddress_UserId] IS NOT NULL");
        }
    }
}
