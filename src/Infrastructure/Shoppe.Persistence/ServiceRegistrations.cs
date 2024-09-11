using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Persistence.Concretes.Repositories;
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
            //services.AddScoped<IDbContextTransaction, IDbContextTransaction>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
