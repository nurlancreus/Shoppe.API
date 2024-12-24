using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Address
{
    public interface IAddressService : IBillingAddressService, IShippingAddressService
    {
    }
}
