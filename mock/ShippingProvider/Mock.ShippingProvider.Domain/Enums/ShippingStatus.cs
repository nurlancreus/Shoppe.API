using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Domain.Enums
{
    public enum ShippingStatus
    {
        Pending,
        Shipped,
        InTransit,
        OutForDelivery,
        Delivered,
        Failed,
        Canceled
    }
}
