using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Features.Command.Auth.Register;
using Shoppe.Domain.Entities.Identity;

namespace Shoppe.Application.Validators.Auth
{
    public class RegisterCommandRequestValidator : AbstractValidator<RegisterCommandRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterCommandRequestValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            // Validate FirstName
            RuleFor(register => register.FirstName)
                .NotEmpty().WithMessage("First Name is required.");

            // Validate LastName
            RuleFor(register => register.LastName)
                .NotEmpty().WithMessage("Last Name is required.");

            // Validate Email (Check if email already exists in UserManager)
            RuleFor(register => register.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MustAsync(async (email, cancellationToken) =>
                    await IsUniqueEmail(email))
                .WithMessage("Email is already registered.");

            // Validate UserName (Check if username already exists in UserManager)
            RuleFor(register => register.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MustAsync(async (username, cancellationToken) =>
                    await IsUniqueUserName(username))
                .WithMessage("Username is already taken.");

            // Validate Password
            RuleFor(register => register.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            // Validate ConfirmPassword matches Password
            RuleFor(register => register.ConfirmPassword)
                .Equal(register => register.Password)
                .WithMessage("Passwords do not match.");
        }

        // Check if the email already exists
        private async Task<bool> IsUniqueEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null; // If user is null, email is unique
        }

        // Check if the username already exists
        private async Task<bool> IsUniqueUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user == null; // If user is null, username is unique
        }
    }
}
