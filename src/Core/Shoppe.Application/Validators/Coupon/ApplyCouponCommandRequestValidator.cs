using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Features.Command.Coupon.Apply;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Coupon
{
    public class ApplyCouponCommandRequestValidator : AbstractValidator<ApplyCouponCommandRequest>
    {
        private readonly ICouponReadRepository _couponReadRepository;
        public ApplyCouponCommandRequestValidator(ICouponReadRepository couponReadRepository)
        {
            _couponReadRepository = couponReadRepository;

            RuleFor(x => x.CouponCode)
                .NotEmpty().WithMessage("Coupon code is required.")
                .Matches(ICouponValidationService.CouponCodeRegex)
                .WithMessage("Coupon code must be in the correct format.")
            .MustAsync(async (code, cancellationToken) =>
                {
                    return !await _couponReadRepository.IsExistAsync(c => c.Code == code, cancellationToken);
                });

            RuleFor(x => x.CouponTarget)
                .NotEmpty().WithMessage("Coupon target is required.");
        }
    }
}
