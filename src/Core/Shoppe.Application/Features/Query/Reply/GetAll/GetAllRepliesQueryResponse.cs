using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.GetAll
{
    public class GetAllRepliesQueryResponse : AppResponseWithPaginatedData<List<GetReplyDTO>>
    {
    }
}
