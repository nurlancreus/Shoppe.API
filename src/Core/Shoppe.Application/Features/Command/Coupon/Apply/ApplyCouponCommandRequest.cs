using MediatR;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Apply
{
    public class ApplyCouponCommandRequest : IRequest<ApplyCouponCommandResponse>
    {
        public string? CouponCode { get; set; } = string.Empty;
        public CouponTarget? CouponTarget { get; set; }
    }
}
