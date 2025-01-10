using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Tag.Create;
using Shoppe.Domain.Enums; // Ensure this namespace contains TagType enum
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Tag
{
    public class CreateTagCommandRequestValidator : AbstractValidator<CreateTagCommandRequest>
    {
        private readonly ITagReadRepository _tagReadRepository;

        public CreateTagCommandRequestValidator(ITagReadRepository tagReadRepository)
        {
            _tagReadRepository = tagReadRepository;

            RuleFor(tag => tag.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(TagConst.MaxNameLength)
                .WithMessage($"Name must be less than {TagConst.MaxNameLength} characters.")
                .MustAsync(async (name, cancellationToken) => !await _tagReadRepository.IsExistAsync(c => c.Name == name, cancellationToken))
                .WithMessage("Tag is already defined.");

            RuleFor(tag => tag.Description)
                .MaximumLength(TagConst.MaxDescLength)
                .WithMessage($"Description must be less than {TagConst.MaxDescLength} characters.")
                .When(tag => !string.IsNullOrEmpty(tag.Description));

            RuleFor(tag => tag.Type)
                .NotEmpty()
                .WithMessage("Tag type is required.")
                .Must(type => Enum.TryParse(typeof(TagType), type, true, out _)) 
                .WithMessage("Invalid tag type specified.");
        }
    }
}
