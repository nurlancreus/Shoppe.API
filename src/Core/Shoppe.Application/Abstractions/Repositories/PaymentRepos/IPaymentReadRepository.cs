using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.PaymentRepos
{
    public interface IPaymentReadRepository : IReadRepository<Payment>
    {
    }
}
