using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Order.Get
{
    public class GetOrderByIdQueryRequest : IRequest<GetOrderByIdQueryResponse>
    {
        public Guid Id {  get; set; }
    }
}
