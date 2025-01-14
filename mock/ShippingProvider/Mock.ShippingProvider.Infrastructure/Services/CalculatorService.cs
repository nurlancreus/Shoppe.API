using Microsoft.Extensions.Options;
using Mock.ShippingProvider.Application.Interfaces;
using Mock.ShippingProvider.Application.Interfaces.Services;
using Mock.ShippingProvider.Application.Options;
using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain.Enums;

namespace Mock.ShippingProvider.Infrastructure.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ShippingRatesSettings _shippingRatesSettings;

        public CalculatorService(IOptions<ShippingRatesSettings> options)
        {
            _shippingRatesSettings = options.Value;
        }
        // Calculate estimated delivery date for a shipment
        public DateTime CalculateEstimatedDelivery(Shipment shipment)
        {
            var distance = ICalculatorService.CalculateDistance(shipment.OriginAddress, shipment.DestinationAddress);

            int baseDays = CalculateDefaultDeliveryTime(distance);  // Default delivery time
            int deliveryTime = CalculateDeliveryTime(baseDays, shipment.ShippingMethod);

            return DateTime.Now.AddDays(deliveryTime);
        }

        // Calculate the shipping cost based on weight, dimensions, distance, and shipping method
        public decimal CalculateShippingCost(Shipment shipment)
        {
            decimal baseCost = shipment.ShippingRate.Rate;  // Use the rate defined for the shipping method
            decimal weightSurcharge = CalculateWeightCost(shipment.ShippingRate.Weight);  // Surcharge based on weight
            decimal sizeSurcharge = CalculateSizeCost(shipment.ShippingRate.Dimensions);  // Surcharge based on volume

            // Calculate distance surcharge based on origin and destination
            var distance = ICalculatorService.CalculateDistance(shipment.OriginAddress, shipment.DestinationAddress);

            decimal distanceSurcharge = CalculateDistanceCost(distance);

            // Adjust cost based on shipping method
            baseCost = CalculateDeliveryCost(baseCost, shipment.ShippingMethod);


            return baseCost + weightSurcharge + sizeSurcharge + distanceSurcharge;
        }

        private int CalculateDefaultDeliveryTime(double distance)
        {
            return _shippingRatesSettings.DefaultDays + (distance / _shippingRatesSettings.DefaultDistance) > 1 ? (int)(distance / _shippingRatesSettings.DefaultDistance) : 0;
        }

        private int CalculateDeliveryTime(int defaultDeliveryDays, ShippingMethod shippingMethod)
        {
            return shippingMethod switch
            {
                ShippingMethod.Standard => defaultDeliveryDays,
                ShippingMethod.Express => (int)Math.Ceiling(defaultDeliveryDays * _shippingRatesSettings.ExpressDeliveryMultiplier),
                ShippingMethod.Overnight => (int)Math.Ceiling(defaultDeliveryDays * _shippingRatesSettings.OvernightDeliveryMultiplier),
                _ => defaultDeliveryDays
            };
        }

        private decimal CalculateDeliveryCost(decimal baseRate, ShippingMethod shippingMethod)
        {
            return shippingMethod switch
            {
                ShippingMethod.Standard => baseRate,
                ShippingMethod.Express => baseRate * _shippingRatesSettings.ExpressRateMultiplier,
                ShippingMethod.Overnight => baseRate * _shippingRatesSettings.OvernightRateMultiplier,
                _ => 0
            };
        }

        private decimal CalculateWeightCost(double weight)
        {
            return (decimal)weight * _shippingRatesSettings.BaseRateForKg;
        }

        private decimal CalculateDistanceCost(double distance)
        {
            return (decimal)distance * _shippingRatesSettings.BaseRateForKm;
        }

        private decimal CalculateSizeCost(string dimensions)
        {
            if (!IValidationService.ValidateDimensions(dimensions))
            {
                return 0;
            }

            int volume = dimensions.Split('x').Select(int.Parse).Aggregate((x, y) => x * y);

            return CalculateSizeCost(volume);
        }

        private decimal CalculateSizeCost(int volume)
        {
            if (!IValidationService.ValidateVolume(volume))
            {
                return 0;
            }

            return volume * _shippingRatesSettings.BaseRateForVolume;
        }
    }
}
