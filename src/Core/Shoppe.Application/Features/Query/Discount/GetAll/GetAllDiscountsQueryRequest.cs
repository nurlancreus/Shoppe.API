using MediatR;
using Shoppe.Application.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Discount.GetAll
{
    public class GetAllDiscountsQueryRequest : PaginationRequestParams, IRequest<GetAllDiscountsQueryResponse>
    {
    }
}
