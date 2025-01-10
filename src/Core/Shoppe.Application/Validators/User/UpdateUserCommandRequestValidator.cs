using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Features.Command.User.Update;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Constants;
using Shoppe.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Services.Session;

namespace Shoppe.Application.Validators.User
{
    public class UpdateUserCommandRequestValidator : AbstractValidator<UpdateUserCommandRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtSession _jwtSession;
        public UpdateUserCommandRequestValidator(UserManager<ApplicationUser> userManager, IJwtSession jwtSession)
        {
            _userManager = userManager;
            _jwtSession = jwtSession;

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
                    if (_jwtSession.GetUserEmail() == email) return true;

                    var userByEmail = await _userManager.FindByEmailAsync(email);

                    return userByEmail == null;
                })
                .WithMessage("Email is already defined, choose another email")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("A valid phone number is required.")
                .MustAsync(async (phone, cancellationToken) =>
                {
                    var userByPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone, cancellationToken);
                    return userByPhone == null;
                })
                .WithMessage("Phone is already defined, choose another phone")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.UserName)
                .MaximumLength(UserConst.MaxUserNameLength)
                .WithMessage($"UserName cannot exceed {UserConst.MaxUserNameLength} characters.")
                .MustAsync(async (username, cancellationToken) =>
                {
                    if (_jwtSession.GetUserName() == username) return true;

                    var userByName = await _userManager.FindByNameAsync(username);
                    return userByName == null;
                })
                .WithMessage("Username is already defined, choose another name")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.NewProfilePictureFile)
                .Must(formFile => FileHelpers.RestrictMimeTypes(formFile!, new[] { "image/jpeg", "image/png" }))
                .WithMessage("Profile picture must be a valid image file.")
                .Must(formFile => FileHelpers.IsSizeOk(formFile!, UserConst.MaxProfilePictureSizeInMB))
                .WithMessage($"Profile picture size must not exceed {UserConst.MaxProfilePictureSizeInMB}MB.")
                .When(x => x.NewProfilePictureFile != null);

            RuleFor(x => x.AlreadyExistingImageId)
                .MustAsync(async (request, id, cancellationToken) =>
                {
                    var user = await _userManager.Users.Include(u => u.ProfilePictureFiles).FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                    return user?.ProfilePictureFiles.Any(pp => pp.Id.ToString() == id) ?? false;
                })
                .WithMessage("Existing image not found, please upload a new one")
                .When(x => x.AlreadyExistingImageId != null);

            // Password validation rules
            //RuleFor(x => x.NewPassword)
            //    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            //    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            //    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            //    .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            //    .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
            //    .When(x => !string.IsNullOrEmpty(x.NewPassword));

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match.")
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required to set a new password.")
                .When(x => !string.IsNullOrEmpty(x.NewPassword));
        }
    }
}
