using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.RequestParams
{
    public class PaginationRequestParams
    {
        public virtual int Page { get; set; } = PaginationConst.Page;
        public virtual int PageSize { get; set; } = PaginationConst.PageSize;
    }
}
