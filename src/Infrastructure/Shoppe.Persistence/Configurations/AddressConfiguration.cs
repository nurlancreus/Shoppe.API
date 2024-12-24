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

            builder
                .HasOne(a => a.Account)
                .WithOne()
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(a => a.UserId)
                .IsUnique(false);

            builder
                .HasIndex(a => new { a.UserId, a.AddressType })
                .IsUnique();

            builder.
                HasIndex(a => new
                { a.FirstName, a.LastName, a.Email, a.Country, a.City, a.PostalCode, a.StreetAddress })
                .IsUnique();
        }
    }
}
