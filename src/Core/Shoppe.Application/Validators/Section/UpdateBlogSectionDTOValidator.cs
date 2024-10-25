using FluentValidation;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Enums;

namespace Shoppe.Application.Validators.Section
{
    public class UpdateBlogSectionDTOValidator : AbstractValidator<UpdateSectionDTO>
    {
        public UpdateBlogSectionDTOValidator()
        {

            // Use BlogConst for blog section validation
            RuleFor(x => x.Title)
                .MaximumLength(BlogConst.MaxTitleLength).WithMessage($"Section title cannot be longer than {BlogConst.MaxTitleLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(BlogConst.MaxDescLength).WithMessage($"Section description cannot be longer than {BlogConst.MaxDescLength} characters.");

            RuleFor(x => x.TextBody)
               .MaximumLength(BlogConst.MaxTextBodyLength).WithMessage($"Section text body cannot be longer than {BlogConst.MaxTextBodyLength} characters.");


            // Common validation for image files
            RuleForEach(x => x.SectionImageFiles)
                .Must(file => file.IsImage()).WithMessage("Only image files are allowed.")
                .Must(file => file.IsSizeOk(BlogConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {BlogConst.MaxFileSizeInMb}MB.")
                .Must(file => file.RestrictExtension(new[] { ".jpg", ".png" })).WithMessage("Allowed file extensions are .jpg, .png.")
                .Must(file => file.RestrictMimeTypes(new[] { "image/jpeg", "image/png" })).WithMessage("Allowed mime types are image/jpeg, image/png.")
                .When(x => x.SectionImageFiles != null && x.SectionImageFiles.Count > 0);

            // Order validation
            RuleFor(x => (int)(x.Order ?? 0))
                .InclusiveBetween(0, 255).WithMessage("You must define section order. (0 - 255)")
                .When(x => x.Order != null);
        }
    }
}
