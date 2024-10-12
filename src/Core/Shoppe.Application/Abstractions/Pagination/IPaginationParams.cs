using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Pagination
{
    public interface IPaginationParams
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}
