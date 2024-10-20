using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Features.Command.User.AssignRoles;
using Shoppe.Domain.Entities.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.User
{
    public class AssignRolesToUserCommandRequestValidator : AbstractValidator<AssignRolesToUserCommandRequest>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AssignRolesToUserCommandRequestValidator(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("At least one role must be assigned.")
                .MustAsync(RoleExists).WithMessage("One or more roles are invalid.");
        }

        private async Task<bool> RoleExists(List<string> roles, CancellationToken cancellationToken)
        {
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return false; 
                }
            }

            return true; 
        }
    }
}
