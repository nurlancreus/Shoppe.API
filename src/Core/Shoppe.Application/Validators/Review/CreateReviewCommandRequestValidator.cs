using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Review.CreateReview;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Review
{
    public class CreateReviewCommandRequestValidator : AbstractValidator<CreateReviewCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateReviewCommandRequestValidator(IProductReadRepository productReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productReadRepository = productReadRepository;
            _httpContextAccessor = httpContextAccessor;

            var isUserAuth = _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

            // Apply rules conditionally based on authentication status
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(ReviewConst.MaxFirstNameLength)
                .WithMessage($"First Name cannot exceed {ReviewConst.MaxFirstNameLength} characters.")
                .When(x => !isUserAuth); // Apply rule if user is not authenticated

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(ReviewConst.MaxLastNameLength)
                .WithMessage($"Last Name cannot exceed {ReviewConst.MaxLastNameLength} characters.")
                .When(x => !isUserAuth); // Apply rule if user is not authenticated

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !isUserAuth); // Apply rule if user is not authenticated

            RuleFor(x => x.Body)
                .MaximumLength(ReviewConst.MaxBodyLength)
                .WithMessage($"Body cannot exceed {ReviewConst.MaxBodyLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Body));

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .MustAsync(async (id, cancellationToken) => await _productReadRepository.IsExist(p => p.Id.ToString() == id, cancellationToken))
                .WithMessage("Product not found.");
        }
    }
}
