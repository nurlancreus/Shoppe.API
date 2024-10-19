using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class CalculatorService : ICalculatorService
    {
        public double? CalculateDiscountedPrice(Product product)
        {
            var discount = product.DiscountMappings.FirstOrDefault(dp => dp.Discount.IsActive)?.Discount;

            if (discount == null) return null;

            return CalculateDiscountedPrice(product.Price, discount.DiscountPercentage);
        }

        public double? CalculateDiscountedPrice(double originalPrice, decimal? discountPercentage)
        {
            if (discountPercentage == null) return null;

            return originalPrice - (originalPrice * (double)discountPercentage / 100);
        }
    }
}
