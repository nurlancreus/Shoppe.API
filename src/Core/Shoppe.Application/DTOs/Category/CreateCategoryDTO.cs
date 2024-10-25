using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Category
{
    public record CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CategoryType Type { get; set; } = CategoryType.Product;
    }
}
