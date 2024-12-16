using Shoppe.Application.DTOs.Product;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetByIds
{
    public class GetProductsByIdQueryResponse : AppResponseWithData<List<GetProductDTO>>
    {
    }
}
