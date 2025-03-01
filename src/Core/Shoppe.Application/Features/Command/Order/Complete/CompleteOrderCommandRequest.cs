using MediatR;

namespace Shoppe.Application.Features.Command.Order.Complete
{
    public class CompleteOrderCommandRequest : IRequest<CompleteOrderCommandResponse>
    {
        public Guid OrderId { get; set; }
    }
}
