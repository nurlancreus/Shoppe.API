using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Toggle
{
    public class ToggleCouponCommandRequest : IRequest<ToggleCouponCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
