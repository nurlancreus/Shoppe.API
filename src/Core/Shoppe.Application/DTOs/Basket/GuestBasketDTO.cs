using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Basket
{
    public record GuestBasketDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
