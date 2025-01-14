using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain.Enums;

namespace Mock.ShippingProvider.Application.Interfaces.Services
{
    public interface ICalculatorService
    {
        DateTime CalculateEstimatedDelivery(Shipment shipment);
        decimal CalculateShippingCost(Shipment shipment);
        public static double CalculateDistance(Address origin, Address destination)
        {
            if (!IValidationService.ValidateAddress(origin) || !IValidationService.ValidateAddress(destination))
            {
                return 0;
            }

            const double EarthRadius = 6371.0; // Earth's radius in kilometers

            // Convert latitude and longitude from degrees to radians
            double lat1 = origin.Latitude * (Math.PI / 180.0);
            double lon1 = origin.Longitude * (Math.PI / 180.0);
            double lat2 = destination.Latitude * (Math.PI / 180.0);
            double lon2 = destination.Longitude * (Math.PI / 180.0);

            // Difference in coordinates
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            // Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance
            double distance = EarthRadius * c; // Distance in kilometers
            return distance;
        }
    }
}
