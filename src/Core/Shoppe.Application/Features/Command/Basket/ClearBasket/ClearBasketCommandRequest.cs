﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Basket.ClearBasket
{
    public class ClearBasketCommandRequest : IRequest<ClearBasketCommandResponse>
    {
    }
}
