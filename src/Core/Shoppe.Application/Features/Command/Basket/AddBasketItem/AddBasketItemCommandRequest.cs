﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.AddBasketItem
{
    public class AddBasketItemCommandRequest : IRequest<AddBasketItemCommandResponse>
    {
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
