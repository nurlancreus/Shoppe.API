using FluentValidation;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Contact.UpdateContact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Contact
{
    public class UpdateContactCommandRequestValidator : AbstractValidator<UpdateContactCommandRequest>
    {
        public UpdateContactCommandRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(ContactConst.MaxFirstNameLength).WithMessage($"First Name cannot exceed {ContactConst.MaxFirstNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(ContactConst.MaxLastNameLength).WithMessage($"Last Name cannot exceed {ContactConst.MaxLastNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Subject)
                .MaximumLength(ContactConst.MaxSubjectLength).WithMessage($"Subject cannot exceed {ContactConst.MaxSubjectLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Subject));

            RuleFor(x => x.Message)
                .MaximumLength(ContactConst.MaxMessageLength).WithMessage($"Message cannot exceed {ContactConst.MaxMessageLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Message));
        }
    }
}
