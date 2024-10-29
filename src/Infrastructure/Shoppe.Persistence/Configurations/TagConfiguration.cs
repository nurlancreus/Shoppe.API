using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Domain.Entities.Reviews;

namespace Shoppe.Persistence.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .HasDiscriminator(c => c.Type)
                .HasValue<BlogTag>("Blog");

        }
    }
}
