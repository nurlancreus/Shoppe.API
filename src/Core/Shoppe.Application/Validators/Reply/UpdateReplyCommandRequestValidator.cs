using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.ReplyRepos; // Assuming you have a Reply repository interface
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Reply.Update;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Reply
{
    public class UpdateReplyCommandRequestValidator : AbstractValidator<UpdateReplyCommandRequest>
    {
        public UpdateReplyCommandRequestValidator()
        {

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Reply ID is required.");

            RuleFor(x => x.Body)
                .MaximumLength(ReplyConst.MaxBodyLength) 
                .WithMessage($"Body cannot exceed {ReplyConst.MaxBodyLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Body)); 
        }
    }
}
