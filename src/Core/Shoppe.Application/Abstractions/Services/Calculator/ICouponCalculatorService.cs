﻿using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Calculator
{
    public interface ICouponCalculatorService
    {
        double CalculateCouponAppliedPrice(Basket basket);
        double CalculateCouponAppliedPrice(Order order);
    }
}
