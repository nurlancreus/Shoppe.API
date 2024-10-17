using MediatR;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Product.UpdateProduct
{
    public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Stock { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public float? Width { get; set; }
        public List<string> Discounts { get; set; } = [];
        public List<string> Materials { get; set; } = [];
        public List<string> Colors { get; set; } = [];
        public List<string> Categories { get; set; } = [];
        public FormFileCollection ProductImages { get; set; } = [];
    }
}
