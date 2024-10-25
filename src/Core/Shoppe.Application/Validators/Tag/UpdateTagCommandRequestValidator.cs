using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Tag.Update;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Tag
{
    public class UpdateTagCommandRequestValidator : AbstractValidator<UpdateTagCommandRequest>
    {
        private readonly ITagReadRepository _tagReadRepository;

        public UpdateTagCommandRequestValidator(ITagReadRepository tagReadRepository)
        {
            _tagReadRepository = tagReadRepository;

            RuleFor(tag => tag.Id)
                .NotEmpty()
                .WithMessage("Tag ID is required.")
                .MustAsync(async (id, cancellationToken) => await _tagReadRepository.IsExistAsync(t => t.Id.ToString() == id, cancellationToken))
                .WithMessage("Tag with the specified ID does not exist.");

            RuleFor(tag => tag.Name)
                .MaximumLength(TagConst.MaxNameLength)
                .WithMessage($"Name must be less than {TagConst.MaxNameLength} characters.")
                .MustAsync(async (request, name, cancellationToken) => !await _tagReadRepository.IsExistAsync(t => t.Name == name && t.Id.ToString() != request.Id, cancellationToken))
                .WithMessage("Tag name is already in use.")
                .When(tag => !string.IsNullOrEmpty(tag.Name)); // Validate if name is provided

            RuleFor(tag => tag.Description)
                .MaximumLength(TagConst.MaxDescLength)
                .WithMessage($"Description must be less than {TagConst.MaxDescLength} characters.")
                .When(tag => !string.IsNullOrEmpty(tag.Description)); // Validate if description is provided
        }
    }
}
