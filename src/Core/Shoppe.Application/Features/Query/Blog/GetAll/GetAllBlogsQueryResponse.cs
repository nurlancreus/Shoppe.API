using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.GetAll
{
    public class GetAllBlogsQueryResponse : AppResponseWithPaginatedData<List<GetBlogDTO>>
    {
    }
}
