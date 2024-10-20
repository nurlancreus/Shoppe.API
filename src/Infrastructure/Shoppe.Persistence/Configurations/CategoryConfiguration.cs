﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                 .HasDiscriminator(c => c.Discriminator)
                 .HasValue<ProductCategory>("Product");

            builder
                .HasDiscriminator(c => c.Discriminator)
                .HasValue<BlogCategory>("Blog");
        }
    }
}
