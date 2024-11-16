using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Params
{
    public interface IProductFilterParams
    {
        string? CategoryName { get; set; }
        double? MinPrice { get; set; }
        double? MaxPrice { get; set; }
        bool? InStock { get; set; }
        string? SortBy { get; set; }
    }
}
