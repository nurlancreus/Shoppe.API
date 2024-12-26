using FluentValidation;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Address.Shipping.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Address.Shipping
{
    public class UpdateShippingAddressCommandValidator : AbstractValidator<UpdateShippingAddressCommandRequest>
    {
        public UpdateShippingAddressCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(AddressConst.MaxFirstNameLength)
                .WithMessage($"First Name must not exceed {AddressConst.MaxFirstNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(AddressConst.MaxLastNameLength)
                .WithMessage($"Last Name must not exceed {AddressConst.MaxLastNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number is not valid.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is not valid.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.StreetAddress)
                .MaximumLength(AddressConst.MaxStreetAddressLength)
                .WithMessage($"Street Address must not exceed {AddressConst.MaxStreetAddressLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.StreetAddress));

            RuleFor(x => x.Country)
                .Must(IAddressValidationService.ValidateCountry!)
                .WithMessage("Country is not allowed.")
                .MaximumLength(AddressConst.MaxCountryLength)
                .WithMessage($"Country must not exceed {AddressConst.MaxCountryLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.City)
                .MaximumLength(AddressConst.MaxCityLength)
                .WithMessage($"City must not exceed {AddressConst.MaxCityLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.PostalCode)
                .Must((request, postalCode) =>
                {
                    if (request.Country != null && postalCode != null)
                        return IAddressValidationService.ValidatePostalCode(postalCode, request.Country);

                    return false;
                })
                .WithMessage("Postal Code is not valid for the specified country.")
                .MaximumLength(AddressConst.MaxPostalCodeLength)
                .WithMessage($"Postal Code must not exceed {AddressConst.MaxPostalCodeLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.PostalCode));
        }
    }
}
