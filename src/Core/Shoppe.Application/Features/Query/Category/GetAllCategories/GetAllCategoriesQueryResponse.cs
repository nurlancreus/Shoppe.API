using Shoppe.Application.DTOs.Category;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Category.GetAllCategories
{
    public class GetAllCategoriesQueryResponse : AppPaginatedResponse<List<GetCategoryDTO>>
    {
    }
}
