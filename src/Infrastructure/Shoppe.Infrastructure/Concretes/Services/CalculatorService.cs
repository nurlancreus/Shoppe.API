using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Flags;

namespace Shoppe.Infrastructure.Concretes.Services
{
    public class CalculatorService : ICalculatorService
    {
        public float CalculateAvgRating(ICollection<Review> reviews)
        {
            if (reviews.Count == 0) return 0;

            var avgRating = reviews.Average(r => (int)r.Rating);

            return (float)Math.Round(avgRating, 2);
        }

        public (double? DiscountedPrice, decimal? GeneralDiscountPercentage) CalculateDiscountedPrice(Product product)
        {

            decimal? effectiveDiscount = CalculateEffectiveDiscount(product.Categories.Cast<IDiscountable>().ToList());

            if ((product.Discount == null || !product.Discount.IsActive || !IDiscountValidationService.CheckIfIsValid(product.Discount)) && effectiveDiscount == null) return (null, null);

            var priceAfterCategoryDiscount = effectiveDiscount != null ? product.Price * (double)(1 - effectiveDiscount) : product.Price;

            var finalPrice = product.Discount != null && product.Discount.IsActive && IDiscountValidationService.CheckIfIsValid(product.Discount) ? Math.Round(priceAfterCategoryDiscount * (double)(1 - product.Discount.DiscountPercentage / 100), 2) : priceAfterCategoryDiscount;

            return (finalPrice, CalculateGeneralDiscountPercentage(effectiveDiscount, product.Discount?.DiscountPercentage));
        }

        public decimal? CalculateEffectiveDiscount(ICollection<IDiscountable> discountables)
        {
            ICollection<decimal> discounts = [];

            if (discountables.Count > 0)
            {
                foreach (var discountable in discountables)
                {
                    if (discountable.Discount != null && discountable.Discount.IsActive && IDiscountValidationService.CheckIfIsValid(discountable.Discount)) discounts.Add(discountable.Discount.DiscountPercentage / 100);
                }
            }

            return discounts.Count > 0 ? 1 - discounts.Aggregate(1m, (acc, d) => acc * (1 - d)) : null;
        }

        private static decimal? CalculateGeneralDiscountPercentage(decimal? categoryDiscount, decimal? productDiscount)
        {
            if (categoryDiscount == null) return productDiscount;
            if (productDiscount == null) return categoryDiscount * 100;

            return (1 - (1 - categoryDiscount.Value) * (1 - productDiscount.Value / 100)) * 100;
        }

        public double CalculateShippingCost(double distance, double baseCost = IShippingCalculatorService.baseShippingCost)
        {

            return baseCost + (distance * IShippingCalculatorService.costPerKm);
        }

        public double CalculateTotalBasketItemsPrice(Basket basket)
        {
            return basket.Items.Sum(bi => bi.Product.Price * bi.Quantity);
        }

        public double CalculateTotalDiscountedBasketItemsPrice(Basket basket)
        {
            if (basket == null || basket.Items == null)
                throw new ArgumentNullException(nameof(basket), "Basket or its items cannot be null.");

            var totalOrderAmount = basket.Items.Sum(i =>
            {
                var (DiscountedPrice, _) = CalculateDiscountedPrice(i.Product);

                return (DiscountedPrice ?? i.Product.Price) * i.Quantity;
            });

            return totalOrderAmount;
        }

        public double CalculateCouponAppliedPrice(Basket basket)
        {
            var basketTotal = CalculateTotalDiscountedBasketItemsPrice(basket);

            if (basket.Coupon == null || !basket.Coupon.IsActive) return basketTotal;

            bool isValid = ICouponValidationService.CheckIfIsValid(basket.Coupon, basketTotal);

            if (!isValid) return basketTotal;

            return basketTotal * (double)(1 - (basket.Coupon.DiscountPercentage / 100));
        }

        public double CalculateCouponAppliedPrice(Order order)
        {
            if (order?.Basket == null)
                throw new ArgumentNullException(nameof(order), "Order or its basket cannot be null.");

            var basketTotal = CalculateCouponAppliedPrice(order.Basket);

            if (order.Coupon == null || !order.Coupon.IsActive)
                return basketTotal;

            bool isOrderCouponValid = ICouponValidationService.CheckIfIsValid(order.Coupon, basketTotal);

            if (!isOrderCouponValid) return basketTotal;

            return basketTotal * (double)(1 - (order.Coupon.DiscountPercentage / 100));
        }
    }
}
