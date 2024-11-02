using FluentValidation;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.Features.Command.Blog.Create;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Validators.Section;
using Shoppe.Domain.Enums;
using Shoppe.Application.Constants;
using Shoppe.Application.Helpers;
using Shoppe.Application.Abstractions.Repositories.TagRepos;

public class CreateBlogCommandRequestValidator : AbstractValidator<CreateBlogCommandRequest>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ITagReadRepository _tagReadRepository;

    public CreateBlogCommandRequestValidator(ICategoryReadRepository categoryReadRepository, ITagReadRepository tagReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _tagReadRepository = tagReadRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Blog title is required.")
            .MaximumLength(BlogConst.MaxTitleLength).WithMessage($"Blog {BlogConst.MaxTitleLength} cannot exceed 100 characters.");

        RuleForEach(x => x.Categories)
            .MustAsync(async (name, cancellationToken) =>
            {
                return await _categoryReadRepository.IsExistAsync(c => c.Name == name, cancellationToken);
            })
            .WithMessage("Category '{PropertyValue}' must be defined and exist in the system.");

        RuleForEach(x => x.Tags)
            .MustAsync(async (name, cancellationToken) =>
            {
                return await _tagReadRepository.IsExistAsync(c => c.Name == name, cancellationToken);
            })
            .WithMessage("Category '{PropertyValue}' must be defined and exist in the system.");

        RuleFor(x => x.CoverImageFile)
                .Must(formFile => FileHelpers.RestrictMimeTypes(formFile!, ["image/jpeg", "image/png"]))
                .WithMessage("Profile picture must be a valid image file.")
                .Must(formFile => FileHelpers.IsSizeOk(formFile!, BlogConst.MaxFileSizeInMb))
                .WithMessage($"Profile picture size must not exceed {BlogConst.MaxFileSizeInMb}MB.");

        RuleFor(x => x.NewSections)
            .NotEmpty().WithMessage("At least one section must be defined.")
            .ForEach(section =>
            {
                section.SetValidator(new CreateBlogSectionDTOValidator());
            });
    }
}
