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
    public class GetProductDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int Stock { get; set; }
        public float Weigth { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public string Material { get; set; } = null!;
        public List<string> Colors { get; set; } = [];
        public List<GetCategoryDTO> Categories { get; set; } = [];
        public List<GetProductImageFileDTO> ProductImages { get; set; } = [];
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
