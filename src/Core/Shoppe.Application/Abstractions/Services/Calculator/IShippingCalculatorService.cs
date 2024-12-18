using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Calculator
{
    public interface IShippingCalculatorService
    {
        const decimal baseShippingCost = 10m; // Base cost for Baku
        const decimal costPerKm = 0.05m; // Cost per kilometer
        decimal CalculateShippingCost(decimal distance, decimal baseCost = baseShippingCost);
    }
}
