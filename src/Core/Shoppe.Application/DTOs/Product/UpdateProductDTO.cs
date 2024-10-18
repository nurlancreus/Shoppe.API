using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Product
{
    public class UpdateProductDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public string? Info { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public double? Price { get; set; }
        public int? Stock { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public float? Width { get; set; }
        public string? DiscountId { get; set; }
        public List<string> Materials { get; set; } = [];
        public List<string> Colors { get; set; } = [];
        public List<string> Categories { get; set; } = [];
        public FormFileCollection ProductImages { get; set; } = [];
    }
}
