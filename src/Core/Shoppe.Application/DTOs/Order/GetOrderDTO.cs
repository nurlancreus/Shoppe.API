using Shoppe.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Order
{
    public record GetOrderDTO
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public double ShippingCost { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        // public GetBasketDTO Basket { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
