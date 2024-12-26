using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Validation
{
    public interface IDiscountValidationService
    {
        public static bool CheckIfIsValid(Discount? discount)
        {
            ArgumentNullException.ThrowIfNull(discount);

            return
                   discount.StartDate < discount.EndDate &&
                   DateTime.UtcNow >= discount.StartDate &&
                   DateTime.UtcNow < discount.EndDate;
        }
    }
}
