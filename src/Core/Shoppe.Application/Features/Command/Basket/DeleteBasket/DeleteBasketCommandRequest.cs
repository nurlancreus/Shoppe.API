using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.DeleteBasket
{
    public class DeleteBasketCommandRequest : IRequest<DeleteBasketCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
