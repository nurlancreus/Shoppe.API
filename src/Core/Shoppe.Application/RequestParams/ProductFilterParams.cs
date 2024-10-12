using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Params;
using Shoppe.Application.Constants;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.RequestParams
{
    public class ProductFilterParams : IProductFilterParams, IPaginationParams
    {
        public string? CategoryName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public string? SortBy { get; set; }
        public int Page { get; init; } = PaginationConst.Page;
        public int PageSize { get; init; } = PaginationConst.PageSize;
    }
}
