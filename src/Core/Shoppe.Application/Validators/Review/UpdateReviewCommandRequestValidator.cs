using FluentValidation;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Review
{
    public class UpdateReviewCommandRequestValidator : AbstractValidator<UpdateReviewCommandRequest>
    {
        public UpdateReviewCommandRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Review ID is required.");

            RuleFor(x => x.Body)
                .MaximumLength(ReviewConst.MaxBodyLength).WithMessage($"Body cannot exceed {ReviewConst.MaxBodyLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Body));

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.")
                .When(x => x.Rating.HasValue);
        }
    }
}
