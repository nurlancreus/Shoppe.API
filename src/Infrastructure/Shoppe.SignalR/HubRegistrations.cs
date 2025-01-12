using Microsoft.AspNetCore.Builder;
using Shoppe.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.SignalR
{
    public static class HubRegistrations
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<AdminHub>("/hubs/admin-hub");
        }
    }
}
