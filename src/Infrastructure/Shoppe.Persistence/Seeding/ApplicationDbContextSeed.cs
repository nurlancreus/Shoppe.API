using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Seeding
{
    public static class ApplicationDbContextSeed
    {
        public static void SeedIdentity(ModelBuilder builder)
        {
            const string adminRoleId = "admin-role-id"; // Use a constant GUID for reproducibility
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = adminRoleId, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" }
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
        }
        public static void SeedCategories(ModelBuilder builder)
        {
            // Seed data for ProductCategory
            builder.Entity<ProductCategory>().HasData(
               new ProductCategory { Id = Guid.NewGuid(), Name = "Necklaces", Description = "Elegant and modern necklaces", Type = "Product", CreatedAt = DateTime.UtcNow },
               new ProductCategory { Id = Guid.NewGuid(), Name = "Earrings", Description = "Stylish earrings for all occasions", Type = "Product", CreatedAt = DateTime.UtcNow },
               new ProductCategory { Id = Guid.NewGuid(), Name = "Bracelets", Description = "Beautiful bracelets in various styles", Type = "Product", CreatedAt = DateTime.UtcNow },
               new ProductCategory { Id = Guid.NewGuid(), Name = "Rings", Description = "Rings for engagement, fashion, and more", Type = "Product", CreatedAt = DateTime.UtcNow },
               new ProductCategory { Id = Guid.NewGuid(), Name = "Brooches", Description = "Unique brooches to complement any outfit", Type = "Product", CreatedAt = DateTime.UtcNow }
               );

            // Seed data for BlogCategory
            builder.Entity<BlogCategory>().HasData(
                new BlogCategory { Id = Guid.NewGuid(), Name = "Jewelry Care", Description = "Tips on how to take care of your jewelry", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "Latest Trends", Description = "Updates on the latest jewelry trends", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "Gemstone Guide", Description = "Learn about different gemstones and their meanings", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "Gift Ideas", Description = "Jewelry gift ideas for various occasions", Type = "Blog", CreatedAt = DateTime.UtcNow },
                new BlogCategory { Id = Guid.NewGuid(), Name = "DIY Jewelry", Description = "Guides and inspiration for making your own jewelry", Type = "Blog", CreatedAt = DateTime.UtcNow }

           );
        }
        public static void SeedTags(ModelBuilder builder)
        {
            builder.Entity<BlogTag>().HasData(
               new BlogTag
               { Id = Guid.NewGuid(), Name = "Fashion", Type = "Blog", CreatedAt = DateTime.UtcNow },
               new BlogTag
               { Id = Guid.NewGuid(), Name = "Jewelry Care", Type = "Blog", CreatedAt = DateTime.UtcNow },
               new BlogTag
               { Id = Guid.NewGuid(), Name = "Gemstones", Type = "Blog", CreatedAt = DateTime.UtcNow },
               new BlogTag
               { Id = Guid.NewGuid(), Name = "DIY Jewelry", Type = "Blog", CreatedAt = DateTime.UtcNow },
               new BlogTag
               { Id = Guid.NewGuid(), Name = "Trends", Type = "Blog", CreatedAt = DateTime.UtcNow }
           );
        }
        public static void SeedAbout(ModelBuilder builder)
        {
            var aboutId = new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb");

            builder.Entity<About>().HasData(new About
            {
                Id = aboutId,
                Name = "Shoppe",
                Description = "Who we are and why we do what we do!",
                Email = "contact@shoppe.com",
                Phone = "123-456-7890",
                CreatedAt = DateTime.UtcNow
            });

            builder.Entity<SocialMediaLink>().HasData
            (
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.Facebook,
                    URL = "https://facebook.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                },
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.X,
                    URL = "https://x.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                },
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.Instagram,
                    URL = "https://instagram.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                },
                new SocialMediaLink
                {
                    Id = Guid.NewGuid(),
                    SocialPlatform = SocialPlatform.Youtube,
                    URL = "https://youtube.com/shoppe",
                    AboutId = aboutId,
                    CreatedAt = DateTime.UtcNow,
                }
            );
        }
        public static void SeedCountries (ModelBuilder builder)
        {
            // Define countries
            var azerbaijan = new Country { Id = Guid.NewGuid(), Name = "Azerbaijan", Code = "AZ" };
            var russia = new Country { Id = Guid.NewGuid(), Name = "Russia", Code = "RU" };
            var georgia = new Country { Id = Guid.NewGuid(), Name = "Georgia", Code = "GE" };
            var turkey = new Country { Id = Guid.NewGuid(), Name = "Turkey", Code = "TR" };
            var iran = new Country { Id = Guid.NewGuid(), Name = "Iran", Code = "IR" };

            builder.Entity<Country>().HasData(azerbaijan, russia, georgia, turkey, iran);

            // Seed cities for each country
            var cities = new List<City>
        {
                new City { Id = Guid.NewGuid(), Name = "Baku", CountryId = azerbaijan.Id },
                new City { Id = Guid.NewGuid(), Name = "Moscow", CountryId = russia.Id  },
                new City { Id = Guid.NewGuid(), Name = "Tbilisi", CountryId = georgia.Id },
                new City { Id = Guid.NewGuid(), Name = "Istanbul", CountryId = turkey.Id },
                new City { Id = Guid.NewGuid(), Name = "Tehran", CountryId = iran.Id },
                new City { Id = Guid.NewGuid(), Name = "Ganja", CountryId = azerbaijan.Id },
                new City { Id = Guid.NewGuid(), Name = "Sumgayit", CountryId = azerbaijan.Id },
                new City { Id = Guid.NewGuid(), Name = "Saint Petersburg", CountryId = russia.Id },
                new City { Id = Guid.NewGuid(), Name = "Novosibirsk", CountryId = russia.Id },
                new City { Id = Guid.NewGuid(), Name = "Batumi", CountryId = georgia.Id },
                new City { Id = Guid.NewGuid(), Name = "Kutaisi", CountryId = georgia.Id },
                new City { Id = Guid.NewGuid(), Name = "Ankara", CountryId = turkey.Id },
                new City { Id = Guid.NewGuid(), Name = "Izmir", CountryId = turkey.Id },
                new City { Id = Guid.NewGuid(), Name = "Mashhad", CountryId = iran.Id },
                new City { Id = Guid.NewGuid(), Name = "Isfahan", CountryId = iran.Id }
        };

            builder.Entity<City>().HasData(cities);
        }
    }
}
