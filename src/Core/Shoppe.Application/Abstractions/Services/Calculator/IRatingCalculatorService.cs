using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Calculator
{
    public interface IRatingCalculatorService
    {
        float CalculateAvgRating(ICollection<Review> reviews);

    }
}
