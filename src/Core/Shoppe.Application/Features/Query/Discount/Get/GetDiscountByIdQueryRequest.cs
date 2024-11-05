using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Discount.Get
{
    public class GetDiscountByIdQueryRequest : IRequest<GetDiscountByIdQueryResponse>
    {
        public Guid? Id { get; set; }
    }
}
