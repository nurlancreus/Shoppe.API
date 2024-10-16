using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Context
{
    public class ShoppeDbContext(DbContextOptions<ShoppeDbContext> dbContextOptions) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(dbContextOptions)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateDateTimesWhileSavingInterceptor();

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ProductConfiguration))!);

            base.OnModelCreating(builder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }
        public DbSet<ProductDimension> ProductDimensions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<ApplicationFile> ApplicationFiles { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; } 
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<BlogImageFile> BlogImageFiles { get; set; }

        private void UpdateDateTimesWhileSavingInterceptor()
        {
            var changedEntries = ChangeTracker.Entries<IBase>().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (var entry in changedEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
