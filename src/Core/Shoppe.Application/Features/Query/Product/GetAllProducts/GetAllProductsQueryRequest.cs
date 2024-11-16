using MediatR;
using Shoppe.Application.Abstractions.Params;
using Shoppe.Application.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetAllProducts
{
    public class GetAllProductsQueryRequest : PaginationRequestParams, IProductFilterParams, IRequest<GetAllProductsQueryResponse>
    {
        public string? CategoryName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public string? SortBy { get; set; }
    }
}
