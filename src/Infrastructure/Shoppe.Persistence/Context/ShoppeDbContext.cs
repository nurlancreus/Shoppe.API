using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Entities.Sections;
using Shoppe.Domain.Entities.Sliders;
using Shoppe.Domain.Entities.Tags;
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

            const string adminRoleId = "admin-role-id"; // Use a constant GUID for reproducibility
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" }
            );

            // Seed the admin user
            const string adminUserId = "admin-user-id"; // Use a constant GUID for reproducibility
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                FirstName = "Nurlan",
                LastName = "Shukurov",
                UserName = "nurlancreus",
                NormalizedUserName = "NURLANCREUS",
                Email = "nurlancreus@example.com",
                NormalizedEmail = "NURLANCREUS@EXAMPLE.COM",
            };

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "qwerty1234");

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Seed user role mapping
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                }
            );

            base.OnModelCreating(builder);
        }
        public DbSet<About> About { get; set; }

        public DbSet<Section> Sections { get; set; }
        public DbSet<AboutSection> AboutSections {  get; set; } 

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<HeroSlider> HeroSlider { get; set; }
        public DbSet<Slide> Slides { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }
        public DbSet<ProductDimension> ProductDimensions { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Payment> Payments { get; set; }


        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<Reply> Replies { get; set; }
        public DbSet<BlogReply> BlogReplies { get; set; }

        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<BlogReaction> BlogReactions { get; set; }
        public DbSet<ReplyReaction> ReplyReactions { get; set; }


        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }


        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        public DbSet<ApplicationFile> ApplicationFiles { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; } 
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<SlideImageFile> SlideImageFiles { get; set; }
        public DbSet<BlogImageFile> BlogImageFiles { get; set; }
        public DbSet<AboutSectionImageFile> AboutSectionImageFiles { get; set; }


        public DbSet<Address> Addresses { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<BillingAddress> BillingAddresses { get; set; }

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
