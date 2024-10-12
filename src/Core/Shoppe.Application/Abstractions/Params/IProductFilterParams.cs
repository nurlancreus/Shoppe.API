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
        public string? CategoryName { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public string? SortBy { get; set; }
    }
}
