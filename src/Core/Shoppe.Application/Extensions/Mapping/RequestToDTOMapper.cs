using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.Features.Command.Category.UpdateCategory;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Mapping
{
    public static class RequestToDTOMapper
    {
        public static CreateProductDTO ToCreateProductDTO(this CreateProductCommandRequest request)
        {
            return new CreateProductDTO
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Weigth = request.Weigth,
                Height = request.Height,
                Width = request.Width,
                Material = request.Material,
                Colors = request.Colors,
                Categories = request.Categories,
                ProductImages = request.ProductImages,
            };
        }

        public static UpdateCategoryDTO ToUpdateCategoryDTO(this UpdateCategoryCommandRequest request)
        {
            return new UpdateCategoryDTO
            {
                Id = request.Id,
                Name = request.Name,
            };
        }
    }
}
