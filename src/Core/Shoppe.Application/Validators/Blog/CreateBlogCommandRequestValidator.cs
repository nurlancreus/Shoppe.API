using FluentValidation;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.Features.Command.Blog.Create;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Validators.Section;
using Shoppe.Domain.Enums;
using Shoppe.Application.Constants;

public class CreateBlogCommandRequestValidator : AbstractValidator<CreateBlogCommandRequest>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public CreateBlogCommandRequestValidator(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Blog title is required.")
            .MaximumLength(BlogConst.MaxTitleLength).WithMessage($"Blog {BlogConst.MaxTitleLength} cannot exceed 100 characters.");

        RuleForEach(x => x.Categories)
            .MustAsync(async (name, cancellationToken) =>
            {
                return await _categoryReadRepository.IsExistAsync(c => c.Name == name, cancellationToken);
            })
            .WithMessage("Category '{PropertyValue}' must be defined and exist in the system.");

        RuleFor(x => x.Sections)
            .NotEmpty().WithMessage("At least one section must be defined.")
            .ForEach(section =>
            {
                section.SetValidator(new CreateSectionDTOValidator(SectionType.Blog.ToString()));
            });
    }
}
