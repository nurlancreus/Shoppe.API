using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        Completed,
        Canceled,
        Refunded,
        Failed,
        Processing,
        Shipped
    }
}
