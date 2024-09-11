using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config => {
                config.RegisterServicesFromAssembly(typeof(ServiceRegistrations).Assembly);
                config.AddOpenBehavior(typeof(CustomValidationBehavior<,>));
            });

            return services;
        }
    }
}
