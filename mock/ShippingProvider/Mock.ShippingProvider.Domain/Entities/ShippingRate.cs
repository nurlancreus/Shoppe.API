using Mock.ShippingProvider.Domain.Entities.Base;
using Mock.ShippingProvider.Domain.Enums;

namespace Mock.ShippingProvider.Domain.Entities
{
    public class ShippingRate : BaseEntity
    {
        public decimal Rate { get; set; }  // Shipping cost based on weight, dimensions, etc.
        public ShippingMethod Method { get; set; }  // Shipping method using enum
        public double Weight { get; set; }  // Weight of the package
        public string Dimensions { get; set; } = string.Empty;  // Package dimensions (e.g., "10x10x10 cm")
        public Guid ShipmentId { get; set; }  // Foreign key to Shipment
        public Shipment Shipment { get; set; } = null!;  // Navigation property to Shipment
    }
}
