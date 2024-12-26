using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Features.Command.Coupon.Create;
using System;

namespace Shoppe.Application.Coupons.Commands.Create
{
    public class CreateCouponCommandRequestValidator : AbstractValidator<CreateCouponCommandRequest>
    {
        private readonly ICouponReadRepository _couponReadRepository;
        public CreateCouponCommandRequestValidator(ICouponReadRepository couponReadRepository)
        {
            _couponReadRepository = couponReadRepository;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Coupon code is required.")
                .Matches(ICouponValidationService.CouponCodeRegex)
                .WithMessage("Coupon code must be in the correct format.")
                .MustAsync(async (code, cancellationToken) =>
                {
                    return !await _couponReadRepository.IsExistAsync(c => c.Code == code, cancellationToken);
                });

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100).WithMessage("Discount percentage must be between 0% and 100%.");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date.");

            RuleFor(x => x.MinimumOrderAmount)
                .GreaterThan(0).WithMessage("Minimum order amount must be greater than 0.");

            RuleFor(x => x.MaxUsage)
                .GreaterThan(0).WithMessage("Max usage must be greater than 0.");
        }
    }
}
