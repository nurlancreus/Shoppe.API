using FluentValidation;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Validation
{
    public interface ICouponValidationService
    {
        public const string CouponCodeRegex = @"^[A-Z0-9-_]+$";
        public static bool CheckIfIsValid(Coupon? coupon)
        {
            ArgumentNullException.ThrowIfNull(coupon);

            return
                   coupon.UsageCount < coupon.MaxUsage &&
                   coupon.StartDate < coupon.EndDate &&
                   DateTime.UtcNow >= coupon.StartDate &&
                   DateTime.UtcNow < coupon.EndDate;
        }

        public static bool CheckIfIsValid(Coupon? coupon, double orderAmount)
        {
            ArgumentNullException.ThrowIfNull(coupon);

            return orderAmount >= coupon.MinimumOrderAmount && CheckIfIsValid(coupon);
        }

        public static bool CheckIfCodeValid(string code, bool throwException = true)
        {
            bool isValid = new Regex(CouponCodeRegex).IsMatch(code);

            if (throwException && !isValid)
            {
                throw new ValidationException("Coupon code is not valid");
            }

            return isValid;
        }
    }
}
