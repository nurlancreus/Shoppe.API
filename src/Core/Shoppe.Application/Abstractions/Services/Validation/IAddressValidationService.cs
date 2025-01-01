using Shoppe.Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Validation
{
    public interface IAddressValidationService
    {
        public static readonly Dictionary<string, (string CountryCode, string PostalCodeRegex)> AllowedCountries = new()
            {
                { "Azerbaijan", ("AZ", @"AZ\s\d{4}$") },
                { "Russia", ("RU", @"^\d{6}$") },
                { "Georgia", ("GE", @"^\d{4}$") },
                { "Iran", ("IR", @"^\d{10}$") },
                { "Turkey", ("TR", @"^\d{5}$") }
            };
        public static bool ValidatePostalCode(string postalCode, string country)
        {

            if (AllowedCountries.ContainsKey(country))
            {
                AllowedCountries.TryGetValue(country, out var value);
                var regex = new Regex(value.PostalCodeRegex);

                return regex.IsMatch(postalCode);

            }

            return false;
        }

        public static bool ValidateCountry(string country)
        {

            return AllowedCountries.ContainsKey(country);
        }

        Task<bool> CheckIfAddressExistAsync(CreateBillingAddressDTO billingAddressDTO, CancellationToken cancellationToken = default);

        Task ValidateBillingAddressAsync(CreateBillingAddressDTO billingAddressDTO, CancellationToken cancellationToken = default);
        void ValidateShippingAddress(CreateShippingAddressDTO shippingAddressDTO);
    }
}
