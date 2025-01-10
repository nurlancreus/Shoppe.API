using FluentValidation;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Slider.RemoveImage;
using Shoppe.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Slider
{
    public class ChangeSlideImageCommandRequestValidator : AbstractValidator<ChangeSlideImageCommandRequest>
    {
        public ChangeSlideImageCommandRequestValidator()
        {
            RuleFor(x => x.SlideId)
            .NotEmpty().WithMessage("SlideId is required.");

            RuleFor(x => x.NewImageFile)
            .NotNull().WithMessage("Slide image file is required.")
            .Must(file => file.IsImage()).WithMessage("Only image files are allowed.")
            .Must(file => file.IsSizeOk(SliderConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {SliderConst.MaxFileSizeInMb}MB.")
            .Must(file => file.RestrictExtension([".jpg", ".png"])).WithMessage("Allowed file extensions are .jpg and .png.")
            .Must(file => file.RestrictMimeTypes(["image/jpeg", "image/png"])).WithMessage("Allowed mime types are image/jpeg and image/png.");
        }
    }
}
