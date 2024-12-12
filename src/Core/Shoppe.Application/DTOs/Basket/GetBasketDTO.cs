using Shoppe.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Basket
{
    public record GetBasketDTO
    {
        public Guid Id { get; set; }
        public GetUserDTO User { get; set; } = null!;
        public List<GetBasketItemDTO> BasketItems { get; set; } = [];
        public double TotalAmount { get; set; } = 0;
        public double? TotalDiscountedAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
