using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppe.Domain.Entities;

namespace Shoppe.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.Order) 
                .WithOne(o => o.Payment) 
                .HasForeignKey<Payment>(p => p.Id) 
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(p => p.Method)
                .HasConversion<string>();

            builder
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}
