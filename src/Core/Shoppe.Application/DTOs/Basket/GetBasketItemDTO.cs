using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Basket
{
    public record GetBasketItemDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double? TotalDiscountedPrice { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
    }
}
