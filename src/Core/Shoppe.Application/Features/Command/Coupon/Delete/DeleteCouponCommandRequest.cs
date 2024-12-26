using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Delete
{
    public class DeleteCouponCommandRequest : IRequest<DeleteCouponCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
