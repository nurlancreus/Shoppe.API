using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Product;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Mapping
{
    public static class DTOToEntityMapper
    {
        public static Product ToProductEntity(this CreateProductDTO createProductDTO)
        {
            return new Product();
        }

        public static Category ToCategoryEntity(this GetCategoryDTO getCategoryDTO)
        {
            return new Category()
            {
                Id = getCategoryDTO.Id,
                Name = getCategoryDTO.Name,
            };
        }
    }
}
