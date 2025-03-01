using MediatR;
using Shoppe.Application.Abstractions.Services;

namespace Shoppe.Application.Features.Command.Order.Complete
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;

        public CompleteOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderService.CompleteOrderAsync(request.OrderId, cancellationToken);

            return new CompleteOrderCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
