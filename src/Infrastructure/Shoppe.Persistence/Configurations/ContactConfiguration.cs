using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Contacts;
using Shoppe.Domain.Entities.Reactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasDiscriminator(c => c.Type)
              .HasValue<RegisteredContact>("Registered")
              .HasValue<UnRegisteredContact>("Unregistered");

            builder
                .Property(c => c.Subject)
                .HasConversion<string>();
        }
    }
}
