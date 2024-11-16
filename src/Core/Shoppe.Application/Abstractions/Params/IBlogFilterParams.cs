using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Params
{
    public interface IBlogFilterParams
    {
        string? CategoryName { get; set; }
        string? TagName { get; set; }
        string? SearchQuery { get; set; }

    }
}
