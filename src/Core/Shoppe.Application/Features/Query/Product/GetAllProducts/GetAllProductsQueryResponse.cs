using Shoppe.Application.DTOs.Product;
using Shoppe.Application.Responses;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetAllProducts
{
    public class GetAllProductsQueryResponse : AppPaginatedResponse<List<GetProductDTO>>
    {
    }
}
