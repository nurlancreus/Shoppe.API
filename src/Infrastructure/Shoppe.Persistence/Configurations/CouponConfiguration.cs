using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder
                .Property(c => c.DiscountPercentage)
                .HasColumnType("decimal(5, 2)") 
                .IsRequired(); 

            builder
                .Property(c => c.MinimumOrderAmount)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();
        }
    }
}
