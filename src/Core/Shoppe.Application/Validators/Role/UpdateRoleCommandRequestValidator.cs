using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Role.Update;
using Shoppe.Domain.Entities.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UpdateRoleCommandRequestValidator : AbstractValidator<UpdateRoleCommandRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UpdateRoleCommandRequestValidator(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId is required.");

        RuleFor(x => x.Name)
            .MaximumLength(RoleConst.MaxNameLength)
            .WithMessage($"Name must not exceed {RoleConst.MaxNameLength} characters.")
            .MustAsync(async (request, name, cancellationToken) =>
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
                if (role == null)
                {
                    return false; 
                }

                if (role.Name!.ToLower() == name.ToLower())
                {
                    return true; 
                }

                bool isRoleExist = await _roleManager.RoleExistsAsync(name);
                if (isRoleExist)
                {
                    return false; 
                }

                return true;
            })
            .WithMessage((request, name) => $"Role name '{name}' is already in use.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(RoleConst.MaxDescLength)
            .WithMessage($"Description must not exceed {RoleConst.MaxDescLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
