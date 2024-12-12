using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Params;
using Shoppe.Application.Constants;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Product
{
    public record ProductFilterParamsDTO : IPaginationParams
    {
        public string? CategoryName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public bool? Discounted {  get; set; }
        public List<SortOption> SortOptions { get; set; } = [];
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}
