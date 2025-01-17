﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetByIds
{
    public class GetProductsByIdQueryRequest : IRequest<GetProductsByIdQueryResponse>
    {
        public string? ProductsIds { get; set; }
    }
}
