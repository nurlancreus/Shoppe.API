using Shoppe.Domain.Entities;
using Shoppe.Domain.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Calculator
{
    public interface IDiscountCalculatorService
    {
        (double? DiscountedPrice, decimal? GeneralDiscountPercentage) CalculateDiscountedPrice(Product product);
        decimal? CalculateEffectiveDiscount(ICollection<IDiscountable> discountables);

    }
}
