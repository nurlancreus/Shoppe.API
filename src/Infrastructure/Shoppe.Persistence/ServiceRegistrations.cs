using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Abstractions.Repositories.BlogImageFileRepos;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.ProductDetailsRepos;
using Shoppe.Application.Abstractions.Repositories.ProductImageFileRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Persistence.Concretes.Repositories;
using Shoppe.Persistence.Concretes.Repositories.BlogImageFileRepos;
using Shoppe.Persistence.Concretes.Repositories.BlogRepos;
using Shoppe.Persistence.Concretes.Repositories.CategoryRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductDetailsRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductImageFileRepos;
using Shoppe.Persistence.Concretes.Repositories.ProductRepos;
using Shoppe.Persistence.Concretes.Services;
using Shoppe.Persistence.Concretes.Services.Auth;
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
            
            services.AddScoped<IBlogImageFileReadRepository, BlogImageFileReadRepository>();
            services.AddScoped<IBlogImageFileWriteRepository, BlogImageFileWriteRepository>();

            services.AddScoped<IBlogReadRepository, BlogReadRepository>();
            services.AddScoped<IBlogWriteRepository, BlogWriteRepository>();

            #endregion
            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Services Registrations
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IInternalAuthService, AuthService>();
            services.AddScoped<IAuthService, AuthService>();
            #endregion

            return services;
        }
    }
}
