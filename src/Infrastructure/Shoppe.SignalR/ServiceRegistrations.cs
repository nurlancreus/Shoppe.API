using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Abstractions.HubServices;
using Shoppe.SignalR.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.SignalR
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterSignalRServices(this IServiceCollection services)
        {
            services.AddTransient<IAdminHubService, AdminHubService>();
            services.AddSignalR();

            return services;
        }
    }
}
