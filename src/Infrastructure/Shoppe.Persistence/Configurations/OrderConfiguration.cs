using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Application.Constants;
using Shoppe.Domain.Entities;

namespace Shoppe.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Note)
                .IsRequired()
                .HasMaxLength(OrderConst.MaxNoteLength);

            builder.Property(o => o.ContactNumber)
                .IsRequired()
                .HasMaxLength(OrderConst.MaxContactNumberLength);


            builder.HasOne(o => o.Basket)
               .WithOne(b => b.Order)
               .HasForeignKey<Order>(o => o.Id) // Assuming Id also foreignkey
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Coupon)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CouponId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // Set behavior when the coupon is deleted

            builder.HasOne(o => o.BillingAddress)
                .WithMany(a => a.Orders)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.ShippingAddress)
                .WithMany(a => a.Orders)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(o => o.OrderStatus)
                .HasConversion<string>();

        }
    }
}
