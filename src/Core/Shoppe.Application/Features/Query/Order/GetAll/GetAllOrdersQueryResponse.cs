using Shoppe.Application.DTOs.Order;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Order.GetAll
{
    public class GetAllOrdersQueryResponse : AppResponseWithPaginatedData<List<GetOrderDTO>>
    {
    }
}
