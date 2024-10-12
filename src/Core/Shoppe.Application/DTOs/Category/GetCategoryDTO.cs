using Shoppe.Application.Responses.Interface;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Category
{
    public class GetCategoryDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Type { get; set; } = null!;
    }
}
