using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.RequestParams
{
    public class PaginationRequestParams : IPaginationParams
    {
        public virtual int Page { get; init; } = PaginationConst.Page;
        public virtual int PageSize { get; init; } = PaginationConst.PageSize;
    }
}
