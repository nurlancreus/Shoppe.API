using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Role.Create;
using Shoppe.Domain.Entities.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CreateRoleCommandRequestValidator : AbstractValidator<CreateRoleCommandRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public CreateRoleCommandRequestValidator(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(RoleConst.MaxNameLength).WithMessage($"Name must not exceed {RoleConst.MaxNameLength} characters.")
            .MustAsync(async (name, cancellationToken) =>
            {
                bool isRoleExist = await _roleManager.RoleExistsAsync(name);
                return !isRoleExist;
            })
            .WithMessage("Role name already exists.");

        RuleFor(x => x.Description)
            .MaximumLength(RoleConst.MaxDescLength).WithMessage($"Description must not exceed {RoleConst.MaxDescLength} characters.");
    }
}
