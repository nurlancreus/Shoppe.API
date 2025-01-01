using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.AddressRepos;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Validation
{
    public class AddressValidationService : IAddressValidationService
    {
        private readonly IAddressReadRepository _addressReadRepository;

        public AddressValidationService(IAddressReadRepository addressReadRepository)
        {
            _addressReadRepository = addressReadRepository;
        }

        public async Task<bool> CheckIfAddressExistAsync(CreateBillingAddressDTO billingAddressDTO, CancellationToken cancellationToken = default)
        {
            var isAddressExist = await _addressReadRepository.IsExistAsync(a =>
                a.StreetAddress == billingAddressDTO.StreetAddress &&
                a.City == billingAddressDTO.City &&
                a.Country == billingAddressDTO.Country &&
                a.PostalCode == billingAddressDTO.PostalCode &&
                a.FirstName == billingAddressDTO.FirstName &&
                a.LastName == billingAddressDTO.LastName &&
                a.Email == billingAddressDTO.Email,
                cancellationToken);

            return isAddressExist;
        }

        public async Task ValidateBillingAddressAsync(CreateBillingAddressDTO billingAddressDTO, CancellationToken cancellationToken = default)
        {
            bool isAddressExist = await CheckIfAddressExistAsync(billingAddressDTO, cancellationToken);

            if (isAddressExist)
                throw new InvalidOperationException("An identical billing address already exists.");


            if (!IAddressValidationService.ValidateCountry(billingAddressDTO.Country))
            {
                throw new InvalidOperationException("We can only ship to Azerbaijan or its neighboring countries.");
            }

            bool isPostalCodeValid = IAddressValidationService.ValidatePostalCode(billingAddressDTO.PostalCode, billingAddressDTO.Country);

            if (!isPostalCodeValid)
            {
                throw new ValidationException("Invalid postal code for the specified country.");
            }
        }

        public void ValidateShippingAddress(CreateShippingAddressDTO shippingAddressDTO)
        {
            if (!IAddressValidationService.ValidateCountry(shippingAddressDTO.Country))
            {
                throw new InvalidOperationException("We can only ship to Azerbaijan or its neighboring countries.");
            }

            bool isPostalCodeValid = IAddressValidationService.ValidatePostalCode(shippingAddressDTO.PostalCode, shippingAddressDTO.Country);

            if (!isPostalCodeValid)
            {
                throw new ValidationException("Invalid postal code for the specified country.");
            }
        }
    }
}
