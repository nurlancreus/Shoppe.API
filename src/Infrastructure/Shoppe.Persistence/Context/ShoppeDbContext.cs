using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Contacts;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Entities.Sliders;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Persistence.Configurations;
using Shoppe.Persistence.Seeding;
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

            ApplicationDbContextSeed.SeedIdentity(builder);
            ApplicationDbContextSeed.SeedAbout(builder);
            ApplicationDbContextSeed.SeedCategories(builder);
            ApplicationDbContextSeed.SeedTags(builder);
            ApplicationDbContextSeed.SeedCountries(builder);

            base.OnModelCreating(builder);
        }
        public DbSet<About> About { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<HeroSlider> HeroSlider { get; set; }
        public DbSet<Slide> Slides { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

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
          
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<RegisteredContact> RegisteredContacts { get; set; }
        public DbSet<UnRegisteredContact> UnRegisteredContacts { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        public DbSet<ApplicationFile> ApplicationFiles { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; } 
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<SlideImageFile> SlideImageFiles { get; set; }
        public DbSet<BlogContentImageFile> BlogImageFiles { get; set; }
        public DbSet<ContentImageFile> ContenImageFiles { get; set; }
        public DbSet<AboutContentImageFile> AboutContentImageFiles { get; set; }

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
