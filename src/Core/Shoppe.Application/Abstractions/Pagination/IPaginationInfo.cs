using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Pagination
{
    public interface IPaginationInfo
    {
        int Page { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; set; }
        int TotalItems { get; set; }
    }
}
