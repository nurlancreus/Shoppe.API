using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Role.Update;
using Shoppe.Domain.Entities.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UpdateRoleCommandRequestValidator : AbstractValidator<UpdateRoleCommandRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateRoleCommandRequestValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId is required.");

        RuleFor(x => x.Name)
            .MaximumLength(RoleConst.MaxNameLength)
            .WithMessage($"Name must not exceed {RoleConst.MaxNameLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name)); 

        RuleFor(x => x.Description)
            .MaximumLength(RoleConst.MaxDescLength)
            .WithMessage($"Description must not exceed {RoleConst.MaxDescLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description)); 

        RuleFor(x => x.UserNames)
            // .NotNull().WithMessage("UserNames cannot be null.")
            .MustAsync(CheckUsernamesExistAsync).WithMessage("Some usernames do not exist.")
            .When(x => x.UserNames != null && x.UserNames.Count != 0); 
    }

    private async Task<bool> CheckUsernamesExistAsync(List<string> usernames, CancellationToken cancellationToken)
    {
        if (usernames == null || usernames.Count == 0)
        {
            return true;
        }

        foreach (var username in usernames)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) 
            {
                return false; 
            }
        }

        return true; 
    }
}
