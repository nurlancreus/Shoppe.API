using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Role.AssignUsers;
using Shoppe.Application.Features.Command.Role.Update;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Role
{
    public class AssignUsersToRoleCommandRequestValidator : AbstractValidator<AssignUsersToRoleCommandRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignUsersToRoleCommandRequestValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");

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
}