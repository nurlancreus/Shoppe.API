using FluentValidation;
using Shoppe.Application.Features.Command.Blog.Update;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Constants;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Application.Helpers;

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

        RuleFor(x => x.Content)
             .MaximumLength(BlogConst.MaxContentLength).WithMessage($"Content cannot be longer than {BlogConst.MaxContentLength} characters.")
             .When(x => !string.IsNullOrEmpty(x.Content));

        RuleForEach(x => x.ContentImages)
         .Must(image => image.ImageFile.IsImage()).WithMessage("Only image files are allowed.")
         .Must(image => image.ImageFile.IsSizeOk(BlogConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {BlogConst.MaxFileSizeInMb}MB.")
         .Must(image => image.ImageFile.RestrictExtension(new[] { ".jpg", ".png" })).WithMessage("Allowed file extensions are .jpg, .png.")
         .Must(image => image.ImageFile.RestrictMimeTypes(new[] { "image/jpeg", "image/png" })).WithMessage("Allowed mime types are image/jpeg, image/png.")
         .Must(image => image.PreviewUrl != null && image.PreviewUrl.Trim() != "").WithMessage("Preview URL is required.")
         .When(x => x.ContentImages != null && x.ContentImages.Count > 0);
    }
}
