using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Calculator
{
    public interface IShippingCalculatorService
    {
        const double baseShippingCost = 10; // Base cost for Baku
        const double costPerKm = 0.05; // Cost per kilometer
        double CalculateShippingCost(double distance, double baseCost = baseShippingCost);
    }
}
