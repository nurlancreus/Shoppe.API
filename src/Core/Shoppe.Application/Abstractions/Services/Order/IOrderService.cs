using Shoppe.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(Guid id, CancellationToken cancellationToken = default);

        Task<GetOrderDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);

        static string GenerateOrderCode()
        {
            return $"ORD-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}
