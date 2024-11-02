using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Files
{
    public class GetAllImagesDTO : IPaginationInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<GetImageFileDTO> Images { get; set; } = [];
    }
}
