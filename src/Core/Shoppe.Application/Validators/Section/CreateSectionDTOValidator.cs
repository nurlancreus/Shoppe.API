using FluentValidation;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Enums;

namespace Shoppe.Application.Validators.Section
{
    public class CreateSectionDTOValidator : AbstractValidator<CreateSectionDTO>
    {
        public CreateSectionDTOValidator(string sectionType)
        {
            // Dynamically choose constants based on SectionType
            if (sectionType == "About")
            {
                // Use AboutConst for "About" section validation
                RuleFor(x => x.Title)
                    //.NotEmpty().WithMessage("Section title cannot be empty.")
                    .MaximumLength(AboutConst.MaxTitleLength).WithMessage($"Section title cannot be longer than {AboutConst.MaxTitleLength} characters.");

                RuleFor(x => x.Description)
                    .MaximumLength(AboutConst.MaxDescLength).WithMessage($"Section description cannot be longer than {AboutConst.MaxDescLength} characters.");

                RuleFor(x => x.TextBody)
                   .MaximumLength(AboutConst.MaxTextBodyLength).WithMessage($"Section text body cannot be longer than {AboutConst.MaxTextBodyLength} characters.");
            }
            else
            {
                // Use BlogConst for blog section validation
                RuleFor(x => x.Title)
                    //.NotEmpty().WithMessage("Section title cannot be empty.")
                    .MaximumLength(BlogConst.MaxTitleLength).WithMessage($"Section title cannot be longer than {BlogConst.MaxTitleLength} characters.");

                RuleFor(x => x.Description)
                    .MaximumLength(BlogConst.MaxDescLength).WithMessage($"Section description cannot be longer than {BlogConst.MaxDescLength} characters.");

                RuleFor(x => x.TextBody)
                   .MaximumLength(BlogConst.MaxTextBodyLength).WithMessage($"Section text body cannot be longer than {BlogConst.MaxTextBodyLength} characters.");
            }

            // Common validation for image files
            RuleForEach(x => x.SectionImageFiles)
                .Must(file => file.IsImage()).WithMessage("Only image files are allowed.")
                .Must(file => file.IsSizeOk(BlogConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {BlogConst.MaxFileSizeInMb}MB.")
                .Must(file => file.RestrictExtension(new[] { ".jpg", ".png" })).WithMessage("Allowed file extensions are .jpg, .png.")
                .Must(file => file.RestrictMimeTypes(new[] { "image/jpeg", "image/png" })).WithMessage("Allowed mime types are image/jpeg, image/png.");

            // Order validation
            RuleFor(x => (int)x.Order)
                .InclusiveBetween(0, 255).WithMessage("You must define section order. (0 - 255)");
        }
    }
}
