using FluentValidation;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Contact.CreateContact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Contact
{
    public class CreateContactCommandRequestValidator : AbstractValidator<CreateContactCommandRequest>
    {
        public CreateContactCommandRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(ContactConst.MaxFirstNameLength).WithMessage($"First Name cannot exceed {ContactConst.MaxFirstNameLength} characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(ContactConst.MaxLastNameLength).WithMessage($"Last Name cannot exceed {ContactConst.MaxLastNameLength} characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.")
                .MaximumLength(ContactConst.MaxSubjectLength).WithMessage($"Subject cannot exceed {ContactConst.MaxSubjectLength} characters.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(ContactConst.MaxMessageLength).WithMessage($"Message cannot exceed {ContactConst.MaxMessageLength} characters.");
        }
    }
}
