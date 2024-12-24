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
        public static bool ValidatePostalCode(string postalCode, string countryCode)
        {
            var (CountryCode, PostalCodeRegex) = AllowedCountries.Values.FirstOrDefault(c => c.CountryCode == countryCode);

            if (CountryCode == null)
            {
                return false;
            }

            var regex = new Regex(PostalCodeRegex);
            return regex.IsMatch(postalCode);
        }
    }
}
