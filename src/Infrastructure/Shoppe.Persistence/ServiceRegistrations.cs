using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Abstractions.Repositories.AddressRepos;
using Shoppe.Application.Abstractions.Repositories.BasketItemRepos;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.BlogImageFileRepos;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.ContactRepos;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Repositories.PaymentRepos;
using Shoppe.Application.Abstractions.Repositories.ProductDetailsRepos;
using Shoppe.Application.Abstractions.Repositories.ProductImageFileRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Persistence.Concretes.Repositories;
using Shoppe.Persistence.Concretes.Repositories.AddressRepos;
using Shoppe.Persistence.Concretes.Repositories.BasketItemRepos;
using Shoppe.Persistence.Concretes.Repositories.BasketRepos;
using Shoppe.Persistence.Concretes.Repositories.BlogImageFileRepos;
using Shoppe.Persistence.Concretes.Repositories.BlogRepos;
using Shoppe.Persistence.Concretes.Repositories.CategoryRepos;
using Shoppe.Persistence.Concretes.Repositories.ContactRepos;
using Shoppe.Persistence.Concretes.Repositories.CouponRepos;
using Shoppe.Persistence.Concretes.Repositories.DiscountRepos;
using Shoppe.Persistence.Concretes.Repositories.OrderRepos;
using Shoppe.Persistence.Concretes.Repositories.PaymentRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductDetailsRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductImageFileRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductRepos;
using Shoppe.Persistence.Concretes.Repositories.ReviewRepos;
using Shoppe.Persistence.Concretes.Services;
using Shoppe.Persistence.Concretes.Services.Auth;
using Shoppe.Persistence.Concretes.Services.Hosted;
using Shoppe.Persistence.Concretes.Services.Shoppe.Persistence.Concretes.Services;
using Shoppe.Persistence.Concretes.UoW;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shoppe.Persistence
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppeDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));

            #region Repo Registrations
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IProductDetailsReadRepository, ProductDetailsReadRepository>();
            services.AddScoped<IProductDetailsWriteRepository, ProductDetailsWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IReviewReadRepository, ReviewReadRepository>();
            services.AddScoped<IReviewWriteRepository, ReviewWriteRepository>();
            
            services.AddScoped<IBlogImageFileReadRepository, BlogImageFileReadRepository>();
            services.AddScoped<IBlogImageFileWriteRepository, BlogImageFileWriteRepository>();

            services.AddScoped<IBlogReadRepository, BlogReadRepository>();
            services.AddScoped<IBlogWriteRepository, BlogWriteRepository>();

            services.AddScoped<IContactReadRepository, ContactReadRepository>();
            services.AddScoped<IContactWriteRepository, ContactWriteRepository>();

            services.AddScoped<ICouponReadRepository, CouponReadRepository>();
            services.AddScoped<ICouponWriteRepository, CouponWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

            services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();
            services.AddScoped<IPaymentWriteRepository, PaymentWriteRepository>();

            services.AddScoped<IDiscountReadRepository, DiscountReadRepository>();
            services.AddScoped<IDiscountWriteRepository, DiscountWriteRepository>();
            
            services.AddScoped<IAddressReadRepository, AddressReadRepository>();
            services.AddScoped<IAddressWriteRepository, AddressWriteRepository>();


            #endregion
            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Services Registrations
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IDiscountService, DiscountService>();

            services.AddScoped<IInternalAuthService, AuthService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IDiscountCalculatorService, CalculatorService>();

            services.AddHostedService<DiscountExpiryBackgroundService>();
            #endregion

            return services;
        }
    }
}
