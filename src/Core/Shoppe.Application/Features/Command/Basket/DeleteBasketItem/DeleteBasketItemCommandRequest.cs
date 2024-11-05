using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.DeleteBasketItem
{
    public class DeleteBasketItemCommandRequest : IRequest<DeleteBasketItemCommandResponse>
    {
        public Guid? BasketItemId { get; set; }

    }
}
