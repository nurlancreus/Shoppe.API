using MediatR;
using Shoppe.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.SyncBasket
{
    public class SyncBasketCommandRequest : IRequest<SyncBasketCommandResponse>
    {
        public IEnumerable<GuestBasketDTO> GuestBasket { get; set; } = [];
    }
}
