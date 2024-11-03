using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.User.ChangeProfilePicture;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.User
{
    public class ChangeProfileImageCommandRequestValidator : AbstractValidator<ChangeProfilePictureCommandRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ChangeProfileImageCommandRequestValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.NewImageFile)
               .Must(formFile => FileHelpers.RestrictMimeTypes(formFile!, ["image/jpeg", "image/png"]))
               .WithMessage("Profile picture must be a valid image file.")
               .Must(formFile => FileHelpers.IsSizeOk(formFile!, UserConst.MaxProfilePictureSizeInMB))
               .WithMessage($"Profile picture size must not exceed {UserConst.MaxProfilePictureSizeInMB}MB.")
               .When(x => x.NewImageFile != null);

            RuleFor(x => x.NewImageId)
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
                .When(x => x.NewImageId != null);
        }
    }
}
