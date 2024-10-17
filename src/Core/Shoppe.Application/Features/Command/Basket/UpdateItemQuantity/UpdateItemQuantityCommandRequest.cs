using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandRequest : IRequest<UpdateItemQuantityCommandResponse>
    {
        public string? BasketItemId { get; set; }
        public int? Quantity { get; set; }
        public bool? Increment { get; set; }

    }
}
