using FluentValidation;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.Features.Command.Blog.Update;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Validators.Section;
using Shoppe.Domain.Enums;
using Shoppe.Application.Constants;
using Shoppe.Application.Abstractions.Repositories.TagRepos;

public class UpdateBlogCommandRequestValidator : AbstractValidator<UpdateBlogCommandRequest>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ITagReadRepository _tagReadRepository;

    public UpdateBlogCommandRequestValidator(ICategoryReadRepository categoryReadRepository, ITagReadRepository tagReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _tagReadRepository = tagReadRepository;

        RuleFor(x => x.Title)
            .MaximumLength(BlogConst.MaxTitleLength).WithMessage($"Blog {BlogConst.MaxTitleLength} cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleForEach(x => x.Categories)
            .MustAsync(async (name, cancellationToken) =>
            {
                return await _categoryReadRepository.IsExistAsync(c => c.Name == name, cancellationToken);
            })
            .WithMessage("Category '{PropertyValue}' must be defined and exist in the system.")
            .When(x => x.Categories.Count > 0);

        RuleForEach(x => x.Tags)
            .MustAsync(async (name, cancellationToken) =>
            {
                return await _tagReadRepository.IsExistAsync(c => c.Name == name, cancellationToken);
            })
            .WithMessage("Category '{PropertyValue}' must be defined and exist in the system.")
            .When(x => x.Tags.Count > 0);

        RuleForEach(x => x.NewSections)
            .SetValidator(new CreateBlogSectionDTOValidator())
            .When(x => x.UpdatedSections != null && x.UpdatedSections.Count > 0);

        RuleForEach(x => x.UpdatedSections)
            .SetValidator(new UpdateBlogSectionDTOValidator())
            .When(x => x.UpdatedSections != null && x.UpdatedSections.Count > 0);
    }
}
