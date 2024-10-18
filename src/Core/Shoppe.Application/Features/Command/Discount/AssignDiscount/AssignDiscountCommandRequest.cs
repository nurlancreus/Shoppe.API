using MediatR;
using Shoppe.Application.Features.Command.Basket.AddBasketItem;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.AssignDiscount
{
    public class AssignDiscountCommandRequest : IRequest<AssignDiscountCommandResponse>
    {
        public string? DiscountId { get; set; }
        public string? EntityId { get; set; }
        public string? EntityType { get; set; } = "Product";
    }
}
