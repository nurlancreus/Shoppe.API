using Microsoft.AspNetCore.SignalR;
using Shoppe.Application.Abstractions.HubServices;
using Shoppe.SignalR.Constants;
using Shoppe.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.SignalR.HubServices
{
    public class AdminHubService : IAdminHubService
    {
        private readonly IHubContext<AdminHub> _hubContext;

        public AdminHubService(IHubContext<AdminHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PaymentSucceededMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.PaymentSucceededMessage, message);
        }
    }
}
