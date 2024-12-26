using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Features.Command.Coupon.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Coupon
{
    public class UpdateCouponCommandRequestValidator : AbstractValidator<UpdateCouponCommandRequest>
    {
        private readonly ICouponReadRepository _couponReadRepository;
        public UpdateCouponCommandRequestValidator(ICouponReadRepository couponReadRepository)
        {
            _couponReadRepository = couponReadRepository;

            RuleFor(x => x.Code)
                .Matches(ICouponValidationService.CouponCodeRegex)
                .WithMessage("Coupon code must be in the correct format.")
                .MustAsync(async (code, cancellationToken) =>
                {
                    return !await _couponReadRepository.IsExistAsync(c => c.Code == code, cancellationToken);
                })
                .When(x => !string.IsNullOrEmpty(x.Code));

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100).WithMessage("Discount percentage must be between 0% and 100%.")
                .When(x => x.DiscountPercentage.HasValue);

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future.")
                .When(x => x.StartDate.HasValue);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date.")
                .When(x => x.EndDate.HasValue);

            RuleFor(x => x.MinimumOrderAmount)
                .GreaterThan(0).WithMessage("Minimum order amount must be greater than 0.")
                .When(x => x.MinimumOrderAmount.HasValue);

            RuleFor(x => x.MaxUsage)
                .GreaterThan(0).WithMessage("Max usage must be greater than 0.")
                .When(x => x.MaxUsage.HasValue);
        }
    }
}
