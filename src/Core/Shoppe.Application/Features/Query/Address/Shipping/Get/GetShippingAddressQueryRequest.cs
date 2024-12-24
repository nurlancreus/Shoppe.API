using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Address.Shipping.Get
{
    public class GetShippingAddressQueryRequest : IRequest<GetShippingAddressQueryResponse>
    {
    }
}
