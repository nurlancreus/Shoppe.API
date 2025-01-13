using Mock.ShippingProvider.Domain.Entities.Base;
using Mock.ShippingProvider.Domain.Enums;
using Mock.ShippingProvider.Domain.Helpers;
using System;
using System.Linq;

namespace Mock.ShippingProvider.Domain.Entities
{
    public class ShippingRate : BaseEntity
    {
        public decimal Rate { get; set; }  // Shipping cost based on weight, dimensions, etc.
        public ShippingMethod Method { get; set; }  // Shipping method using enum
        public decimal Weight { get; set; }  // Weight of the package
        public string Dimensions { get; set; } = string.Empty;  // Package dimensions (e.g., "10x10x10 cm")
        public Guid ShipmentId { get; set; }  // Foreign key to Shipment
        public Shipment Shipment { get; set; } = null!;  // Navigation property to Shipment

        // Calculate estimated delivery date for a shipment
        public DateTime CalculateEstimatedDelivery()
        {
            var distance = ShippingCalculator.CalculateDistance(Shipment.OriginAddress, Shipment.DestinationAddress);

            int baseDays = 5;  // Default delivery time
            if (Shipment.ShippingMethod == ShippingMethod.Express)
            {
                baseDays = 2;  // Express shipping reduces delivery time
            }

            // Add delivery time based on distance and shipping method
            baseDays += (distance / 100) > 1 ? (int)(distance / 100) : 0;

            return DateTime.Now.AddDays(baseDays);
        }

        // Calculate the shipping cost based on weight, dimensions, distance, and shipping method
        public decimal CalculateShippingCost()
        {
            decimal baseCost = Rate;  // Use the rate defined for the shipping method
            decimal weightSurcharge = Weight * 0.5m;  // Surcharge based on weight
            decimal sizeSurcharge = Dimensions.Split('x').Select(int.Parse).Aggregate((x, y) => x * y) * 0.05m;  // Surcharge based on volume

            // Calculate distance surcharge based on origin and destination
            var distance = ShippingCalculator.CalculateDistance(Shipment.OriginAddress, Shipment.DestinationAddress);
            decimal distanceSurcharge = (decimal)(distance * 0.1);

            // Adjust cost based on shipping method
            if (Shipment.ShippingMethod == ShippingMethod.Express)
            {
                baseCost *= 1.5m;  // Express shipping is 50% more expensive
            }

            return baseCost + weightSurcharge + sizeSurcharge + distanceSurcharge;
        }
    }
}
