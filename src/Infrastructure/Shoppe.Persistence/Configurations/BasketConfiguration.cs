using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.UserId)
                .IsRequired(); 

            builder.HasOne(b => b.User)
                .WithMany(u => u.Baskets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(b => b.Items)
                .WithOne(i => i.Basket) 
                .HasForeignKey(i => i.BasketId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Coupon)
                .WithMany(c => c.Baskets)
                .HasForeignKey(o => o.CouponId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            // Configuration for Quantity
            builder.Property(bi => bi.Quantity)
                .IsRequired() // Make sure Quantity is required
                .HasDefaultValue(0); // Optional: Set default value to 0

            builder.ToTable(bi => bi.HasCheckConstraint("CK_BasketItem_Quantity", "Quantity >= 0"));
        }
    }
}
