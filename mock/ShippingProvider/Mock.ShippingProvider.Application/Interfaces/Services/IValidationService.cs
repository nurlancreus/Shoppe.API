using Mock.ShippingProvider.Domain.Entities;
using System.Text.RegularExpressions;

namespace Mock.ShippingProvider.Application.Interfaces
{
    public partial interface IValidationService
    {
        private const string DimensionPattern = @"^\s*(\d+)\s*[xX]\s*(\d+)\s*[xX]\s*(\d+)\s*(cm|CM)?\s*$";

        public static bool ValidateDimensions(string dimensions)
        {
            return DimensionsPatternRegex().IsMatch(dimensions);
        }

        public static bool ValidateVolume(double volume)
        {
            return volume > 0;
        }

        public static bool ValidateAddress(Address address)
        {
            return address != null &&
                   !string.IsNullOrWhiteSpace(address.Street) &&
                   !string.IsNullOrWhiteSpace(address.City) &&
                   address.Latitude >= -90 && address.Latitude <= 90 &&
                   address.Longitude >= -180 && address.Longitude <= 180;
        }

        [GeneratedRegex(DimensionPattern)]
        private static partial Regex DimensionsPatternRegex();
    }
}
