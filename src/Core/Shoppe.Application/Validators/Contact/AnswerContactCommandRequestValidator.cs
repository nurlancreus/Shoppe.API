using FluentValidation;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Contact.AnswerContact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Contact
{
    public class AnswerContactCommandRequestValidator : AbstractValidator<AnswerContactCommandRequest>
    {
        public AnswerContactCommandRequestValidator()
        {
            RuleFor(x => x.ContactId)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.");
        }
    }
}
