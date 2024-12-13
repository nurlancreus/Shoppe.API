using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Product
{
    public record GetProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public Guid? DiscountId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public double? DiscountedPrice { get; set; }
        public int Stock { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public List<string> Materials { get; set; } = [];
        public List<string> Colors { get; set; } = [];
        public List<GetCategoryDTO> Categories { get; set; } = [];
        public List<GetImageFileDTO> ProductImages { get; set; } = [];
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
