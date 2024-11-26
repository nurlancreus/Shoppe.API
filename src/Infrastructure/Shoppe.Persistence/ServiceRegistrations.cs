using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Application.Abstractions.Repositories.AddressRepos;
using Shoppe.Application.Abstractions.Repositories.BasketItemRepos;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.ContactRepos;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Repositories.PaymentRepos;
using Shoppe.Application.Abstractions.Repositories.ProductImageFileRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Repositories.ReactionRepos;
using Shoppe.Application.Abstractions.Repositories.ReplyRepos;
using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Application.Abstractions.Repositories.SliderRepository;
using Shoppe.Application.Abstractions.Repositories.SlideRepos;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Application.Abstractions.Repositories.UserProfilePictureFileRepos;
using Shoppe.Application.Abstractions.Repositories.UserProfilePictureRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using Shoppe.Persistence.Concretes.Repositories;
using Shoppe.Persistence.Concretes.Repositories.AboutRepos;
using Shoppe.Persistence.Concretes.Repositories.AddressRepos;
using Shoppe.Persistence.Concretes.Repositories.BasketItemRepos;
using Shoppe.Persistence.Concretes.Repositories.BasketRepos;
using Shoppe.Persistence.Concretes.Repositories.BlogRepos;
using Shoppe.Persistence.Concretes.Repositories.CategoryRepos;
using Shoppe.Persistence.Concretes.Repositories.ContactRepos;
using Shoppe.Persistence.Concretes.Repositories.CouponRepos;
using Shoppe.Persistence.Concretes.Repositories.DiscountRepos;
using Shoppe.Persistence.Concretes.Repositories.OrderRepos;
using Shoppe.Persistence.Concretes.Repositories.PaymentRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductImageFileRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductRepos;
using Shoppe.Persistence.Concretes.Repositories.ReactionRepos;
using Shoppe.Persistence.Concretes.Repositories.ReplyRepos;
using Shoppe.Persistence.Concretes.Repositories.ReviewRepos;
using Shoppe.Persistence.Concretes.Repositories.SliderRepos;
using Shoppe.Persistence.Concretes.Repositories.TagRepos;
using Shoppe.Persistence.Concretes.Repositories.UserProfilePictureFileRepos;
using Shoppe.Persistence.Concretes.Services;
using Shoppe.Persistence.Concretes.Services.Auth;
using Shoppe.Persistence.Concretes.Services.Hosted;
using Shoppe.Persistence.Concretes.UoW;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Persistence.Concretes.Repositories.SlideRepos;
using Shoppe.Application.Abstractions.Repositories.SocialMediaRepos;
using Shoppe.Persistence.Concretes.Repositories.SocialMediaRepos;
using Shoppe.Application.Abstractions.Repositories.FileRepos;
using Shoppe.Persistence.Concretes.Repositories.FileRepos;
using Shoppe.Application.Abstractions.Services.Files.Images;
using Shoppe.Persistence.Concretes.Services.Files;
using Shoppe.Application.Abstractions.Services.Files;


namespace Shoppe.Persistence
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppeDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
                options.EnableSensitiveDataLogging();
            });

            #region Repo Registrations
            services.AddScoped<IAboutReadRepository, AboutReadRepository>();
            services.AddScoped<IAboutWriteRepository, AboutWriteRepository>();

            services.AddScoped<ISliderReadRepository, SliderReadRepository>();
            services.AddScoped<ISliderWriteRepository, SliderWriteRepository>();

            services.AddScoped<ISlideReadRepository, SlideReadRepository>();
            services.AddScoped<ISlideWriteRepository, SlideWriteRepository>();

            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IUserProfilePictureFileReadRepository, UserProfilePictureFileReadRepository>();
            services.AddScoped<IUserProfilePictureFileWriteRepository, UserProfilePictureFileWriteRepository>();

            services.AddScoped<IReviewReadRepository, ReviewReadRepository>();
            services.AddScoped<IReviewWriteRepository, ReviewWriteRepository>();

            services.AddScoped<IReplyReadRepository, ReplyReadRepository>();
            services.AddScoped<IReplyWriteRepository, ReplyWriteRepository>();

            services.AddScoped<IReactionReadRepository, ReactionReadRepository>();
            services.AddScoped<IReactionWriteRepository, ReactionWriteRepository>();

            services.AddScoped<ITagReadRepository, TagReadRepository>();
            services.AddScoped<ITagWriteRepository, TagWriteRepository>();

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

            services.AddScoped<ISocialMediaLinkReadRepository, SocialMediaLinkReadRepository>();
            services.AddScoped<ISocialMediaLinkWriteRepository, SocialMediaLinkWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();


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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBlogService, BlogService>();

            services.AddScoped<ITagService, TagService>();

            services.AddScoped<IReactionService, ReactionService>();

            services.AddScoped<IReplyService, ReplyService>();
            services.AddScoped<ISliderService, SliderService>();

            services.AddScoped<IApplicationImageFileService, ApplicationFileService>();
            services.AddScoped<IApplicationFileService, ApplicationFileService>();

            services.AddHostedService<DiscountExpiryBackgroundService>();
            #endregion

            return services;
        }
    }
}
