using Application.Interfaces.Services;
using MediatR;
using Shoppe.Domain.Enums;

namespace Shoppe.Application.Features.Command.Order.CreateCheckout
{
    public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommandRequest, CreateCheckoutCommandResponse>
    {
        private readonly ICheckoutService _checkoutService;

        public CreateCheckoutCommandHandler(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public async Task<CreateCheckoutCommandResponse> Handle(CreateCheckoutCommandRequest request, CancellationToken cancellationToken)
        {
            await _checkoutService.CheckoutAsync(new DTOs.Checkout.CreateCheckoutDTO
            {
                BasketId = request.BasketId,
                BillingAddress = request.BillingAddress,
                ShippingAddress = request.ShippingAddress,
                CouponCode = request.CouponCode,
                OrderNote = request.OrderNote,
                PaymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod),
                Phone = request.Phone,
                Shipment = request.Shipment
            }, cancellationToken);

            return new CreateCheckoutCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
