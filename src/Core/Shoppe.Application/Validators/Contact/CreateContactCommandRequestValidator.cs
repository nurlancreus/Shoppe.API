using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Contact.CreateContact;
using Shoppe.Domain.Enums;
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
                .MaximumLength(ContactConst.MaxFirstNameLength).WithMessage($"First Name cannot exceed {ContactConst.MaxFirstNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(ContactConst.MaxLastNameLength).WithMessage($"Last Name cannot exceed {ContactConst.MaxLastNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.")
            .Must((subject) =>
                {
                    if (Enum.TryParse(subject, true, out ContactSubject _)) return true;
                    return false;
                })
                .WithMessage("Subject is not defined")
                .MaximumLength(ContactConst.MaxSubjectLength).WithMessage($"Subject cannot exceed {ContactConst.MaxSubjectLength} characters.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(ContactConst.MaxMessageLength).WithMessage($"Message cannot exceed {ContactConst.MaxMessageLength} characters.");
        }
    }
}
