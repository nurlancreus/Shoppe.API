using FluentValidation;
using Shoppe.Application.DTOs.Slide;
using Shoppe.Application.Helpers;
using Shoppe.Application.Constants;
using Shoppe.Application.Extensions.Helpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Shoppe.Application.Features.Command.Slider.AddSlide;

public class AddSlideToSliderCommandRequestValidator : AbstractValidator<AddSlideToSliderCommandRequest>
{
    public AddSlideToSliderCommandRequestValidator()
    {
        RuleFor(x => x.SliderId)
            .NotEmpty().WithMessage("SliderId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required.")
            .MaximumLength(500).WithMessage("Body cannot be longer than 500 characters.");

        RuleFor(x => x.URL)
            .NotEmpty().WithMessage("URL is required.")
            .Must(UrlHelpers.BeAValidUrl).WithMessage("Invalid URL format.");

        RuleFor(x => x.ButtonText)
            .NotEmpty().WithMessage("Button text is required.")
            .MaximumLength(50).WithMessage("Button text cannot be longer than 50 characters.");

        RuleFor(x => x.SlideImageFile)
            .NotNull().WithMessage("Slide image file is required.")
            .Must(file => file.IsImage()).WithMessage("Only image files are allowed.")
            .Must(file => file.IsSizeOk(SliderConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {SliderConst.MaxFileSizeInMb}MB.")
            .Must(file => file.RestrictExtension([".jpg", ".png"])).WithMessage("Allowed file extensions are .jpg and .png.")
            .Must(file => file.RestrictMimeTypes(["image/jpeg", "image/png"])).WithMessage("Allowed mime types are image/jpeg and image/png.");

        // Order validation
        RuleFor(x => (int)x.Order)
            .InclusiveBetween(0, 255).WithMessage("You must define section order. (0 - 255)");
    }
}
