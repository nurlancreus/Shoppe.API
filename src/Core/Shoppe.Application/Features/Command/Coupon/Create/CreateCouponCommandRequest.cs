using MediatR;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Coupon.Create
{
    public class CreateCouponCommandRequest : IRequest<CreateCouponCommandResponse>
    {
        public string Code { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double MinimumOrderAmount { get; set; }
        public int MaxUsage { get; set; }
    }
}
