using MediatR;
using Shoppe.Application.DTOs.Address;
using Shoppe.Application.DTOs.Shipment;
using Shoppe.Domain.Enums;
namespace Shoppe.Application.Features.Command.Order.CreateCheckout
{
    public class CreateCheckoutCommandRequest : IRequest<CreateCheckoutCommandResponse>
    {
        public Guid BasketId { get; set; }
        public CreateBillingAddressDTO? BillingAddress { get; set; }
        public CreateShippingAddressDTO? ShippingAddress { get; set; }
        public string OrderNote { get; set; } = string.Empty;
        public CreateShipmentDTO? Shipment { get; set; }
        public string? CouponCode { get; set; }
        public string? Phone { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
