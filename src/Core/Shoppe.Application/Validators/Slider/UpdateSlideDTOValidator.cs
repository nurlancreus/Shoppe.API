using FluentValidation;
using Shoppe.Application.DTOs.Slide;
using Shoppe.Application.Helpers;
using Shoppe.Application.Constants;
using Shoppe.Application.Extensions.Helpers;

public class UpdateSlideDTOValidator : AbstractValidator<UpdateSlideDTO>
{
    public UpdateSlideDTOValidator()
    {
        // Validate Title - optional
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        // Validate Body - optional
        RuleFor(x => x.Body)
            .MaximumLength(500).WithMessage("Body cannot be longer than 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Body));

        // Validate URL - optional
        RuleFor(x => x.URL)
            .Must((url) => UrlHelpers.BeAValidUrl(url ?? string.Empty)).WithMessage("Invalid URL format.")
            .When(x => !string.IsNullOrEmpty(x.URL));

        // Validate ButtonText - optional
        RuleFor(x => x.ButtonText)
            .MaximumLength(50).WithMessage("Button text cannot be longer than 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.ButtonText));

        // Validate SlideImageFile - optional
        RuleFor(x => x.SlideImageFile)
            .Must(file => file.IsImage()).WithMessage("Only image files are allowed.")
            .Must(file => file.IsSizeOk(SliderConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {SliderConst.MaxFileSizeInMb}MB.")
            .Must(file => file.RestrictExtension([".jpg", ".png"])).WithMessage("Allowed file extensions are .jpg and .png.")
            .Must(file => file.RestrictMimeTypes(["image/jpeg", "image/png"])).WithMessage("Allowed mime types are image/jpeg and image/png.")
            .When(x => x.SlideImageFile != null);
    }
}
