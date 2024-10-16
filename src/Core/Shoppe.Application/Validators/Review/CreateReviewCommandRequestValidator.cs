using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Review
{
    public class CreateReviewCommandRequestValidator : AbstractValidator<CreateReviewCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IBlogReadRepository _blogReadRepository;

        public CreateReviewCommandRequestValidator(IProductReadRepository productReadRepository, IBlogReadRepository blogReadRepository)
        {
            _productReadRepository = productReadRepository;
            _blogReadRepository = blogReadRepository;

            RuleFor(x => x.Body)
                .MaximumLength(ReviewConst.MaxBodyLength)
                .WithMessage($"Body cannot exceed {ReviewConst.MaxBodyLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Body));

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("type is required.")
                .Must(type => Enum.TryParse(typeof(ReviewType), type?.ToString(), true, out _))
                .WithMessage("Invalid review type. Valid types are: 'Product', 'Blog'.");

            RuleFor(x => x.EntityId)
                .NotEmpty().WithMessage("Entity ID is required.")
                .MustAsync(ValidateEntityIdAsync).WithMessage("Entity not found.");
        }

        private async Task<bool> ValidateEntityIdAsync(CreateReviewCommandRequest request, string? entityId, CancellationToken cancellationToken)
        {
            if (request.Type == ReviewType.Product.ToString().ToLower())
            {
                return await _productReadRepository.IsExist(p => p.Id.ToString() == entityId, cancellationToken);
            }
            //else if (request.Type == ReviewType.Blog.ToString())
            //{
            //    return await _blogReadRepository.IsExist(b => b.Id.ToString() == entityId, cancellationToken);
            //}

            return false; 
        }
    }
}
