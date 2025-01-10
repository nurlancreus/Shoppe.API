using FluentValidation;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Address.Billing.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Address.Billing
{
    public class CreateBillingAddressCommandValidator : AbstractValidator<CreateBillingAddressCommandRequest>
    {
        public CreateBillingAddressCommandValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(AddressConst.MaxFirstNameLength).WithMessage($"First Name must not exceed {AddressConst.MaxFirstNameLength} characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(AddressConst.MaxLastNameLength).WithMessage($"Last Name must not exceed {AddressConst.MaxLastNameLength} characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number is not valid.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.StreetAddress)
                .NotEmpty().WithMessage("Street Address is required.")
                .MaximumLength(AddressConst.MaxStreetAddressLength).WithMessage($"Street Address must not exceed {AddressConst.MaxStreetAddressLength} characters.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .Must(IAddressValidationService.ValidateCountry)
                .WithMessage("Country is not allowed.")
                .MaximumLength(AddressConst.MaxCountryLength).WithMessage($"Country must not exceed {AddressConst.MaxCountryLength} characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(AddressConst.MaxCityLength).WithMessage($"City must not exceed {AddressConst.MaxCityLength} characters.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal Code is required.")
                 .Must((request, postalCode) =>
                 {
                     return IAddressValidationService.ValidatePostalCode(postalCode, request.Country);
                 })
                .WithMessage("Postal Code is not valid for the specified country.")
                .MaximumLength(AddressConst.MaxPostalCodeLength).WithMessage($"Postal Code must not exceed {AddressConst.MaxPostalCodeLength} characters.");
        }
    }
}
