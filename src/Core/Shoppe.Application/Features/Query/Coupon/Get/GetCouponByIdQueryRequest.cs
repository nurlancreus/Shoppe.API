using MediatR;
using Shoppe.Application.Features.Query.Coupon.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Coupon.Get
{
    public class GetCouponByIdQueryRequest : IRequest<GetCouponByIdQueryResponse>
    {
        public Guid? Id { get; set; }
    }
}
