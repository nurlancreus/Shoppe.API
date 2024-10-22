using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Features.Command.User.Update;
using Shoppe.Application.Helpers;
using Shoppe.Application.Constants;
using Microsoft.AspNetCore.Identity;
using Shoppe.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Shoppe.Application.Validators.User
{
    public class UpdateUserCommandRequestValidator : AbstractValidator<UpdateUserCommandRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdateUserCommandRequestValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

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
                .MustAsync(async (email, cancellationToken) =>
                {
                    var userByEmail = await _userManager.FindByEmailAsync(email);

                    if (userByEmail != null)
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage("Email is already defined, choose another email")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("A valid phone number is required.")
                .MustAsync(async (phone, cancellationToken) =>
                {
                    var userByPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone, cancellationToken);

                    if (userByPhone != null) return false;

                    return true;
                })
                .WithMessage("Phone is already defined, choose another phone")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.UserName)
                .MaximumLength(UserConst.MaxUserNameLength)
                .WithMessage($"UserName cannot exceed {UserConst.MaxUserNameLength} characters.")
                .MustAsync(async (username, cancellationToken) =>
                {
                    var userByName = await _userManager.FindByNameAsync(username);

                    if (userByName != null)
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage("Username is already defined, choose another name")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.NewProfilePictureFile)
                .Must(formFile => FileHelpers.RestrictMimeTypes(formFile!, ["image/jpeg", "image/png"]))
                .WithMessage("Profile picture must be a valid image file.")
                .Must(formFile => FileHelpers.IsSizeOk(formFile!, UserConst.MaxProfilePictureSizeInMB))
                .WithMessage($"Profile picture size must not exceed {UserConst.MaxProfilePictureSizeInMB}MB.")
                .When(x => x.NewProfilePictureFile != null);

            RuleFor(x => x.AlreadyExistingImageId)
                .MustAsync(async (request, id, cancellationToken) =>
                {
                    var user = await _userManager.FindByIdAsync(request.UserId!);

                    if (user != null)
                    {

                        var existingImage = user.ProfilePictureFiles.FirstOrDefault(pp => pp.Id.ToString() == id);

                        if (existingImage != null)
                        {
                            return true;
                        }
                    }

                    return false;

                }).WithMessage("Existing image not found, please upload new one")
                .When(x => x.AlreadyExistingImageId != null);
        }
    }
}
