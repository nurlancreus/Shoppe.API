using MediatR;
using Shoppe.Application.RequestParams;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Tag.GetAll
{
    public class GetAllTagsQueryRequest : PaginationRequestParams, IRequest<GetAllTagsQueryResponse>
    {
        public string? Type { get; set; }

    }
}
