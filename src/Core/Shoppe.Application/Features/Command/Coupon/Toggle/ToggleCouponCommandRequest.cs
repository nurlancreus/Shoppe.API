using MediatR;

namespace Shoppe.Application.Features.Command.Coupon.Toggle
{
    public class ToggleCouponCommandRequest : IRequest<ToggleCouponCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
