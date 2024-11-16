using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Blog
{
    public record BlogFilterParamsDTO : IBlogFilterParams, IPaginationParams
    {
        public string? CategoryName { get; set; }
        public string? TagName { get; set; }
        public string? SearchQuery { get; set; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}
