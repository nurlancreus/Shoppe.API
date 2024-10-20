using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Features.Command.User.Update;
using Shoppe.Application.Helpers;
using Shoppe.Application.Constants;

namespace Shoppe.Application.Validators.User
{
    public class UpdateUserCommandRequestValidator : AbstractValidator<UpdateUserCommandRequest>
    {
        public UpdateUserCommandRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.FirstName)
                .MaximumLength(UserConst.MaxFirstNameLength)
                .WithMessage($"FirstName cannot exceed {UserConst.MaxFirstNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(UserConst.MaxLastNameLength)
                .WithMessage($"LastName cannot exceed {UserConst.MaxLastNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email address is required.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("A valid phone number is required.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.UserName)
                .MaximumLength(UserConst.MaxUserNameLength)
                .WithMessage($"UserName cannot exceed {UserConst.MaxUserNameLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.ProfilePictureFile)
                .Must(formFile => FileHelpers.RestrictMimeTypes(formFile!, ["image/jpeg", "image/png"]))
                .WithMessage("Profile picture must be a valid image file.")
                .Must(formFile => FileHelpers.IsSizeOk(formFile!, UserConst.MaxProfilePictureSizeInMB))
                .WithMessage($"Profile picture size must not exceed {UserConst.MaxProfilePictureSizeInMB}MB.")
                .When(x => x.ProfilePictureFile != null);
        }
    }
}
