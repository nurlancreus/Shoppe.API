using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Tag.GetAll
{
    public class GetAllTagsQueryResponse : AppResponseWithPaginatedData<List<GetTagDTO>>
    {
    }
}
