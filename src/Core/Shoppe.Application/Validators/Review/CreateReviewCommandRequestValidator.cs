using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Domain.Constants;
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

            RuleFor(x => x.EntityId)
                .NotEmpty().WithMessage("Entity ID is required.")
                .MustAsync(ValidateEntityIdAsync).WithMessage("Entity not found.");
        }

        private async Task<bool> ValidateEntityIdAsync(CreateReviewCommandRequest request, Guid? entityId, CancellationToken cancellationToken)
        {
            if (request.Type == ReviewType.Product)
            {
                return await _productReadRepository.IsExistAsync(p => p.Id == entityId, cancellationToken);
            }
            //else if (request.Type == ReviewType.Blog.ToString())
            //{
            //    return await _blogReadRepository.IsExist(b => b.Id.ToString() == entityId, cancellationToken);
            //}

            return false; 
        }
    }
}
