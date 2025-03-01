using MediatR;
using Shoppe.Application.RequestParams;

namespace Shoppe.Application.Features.Query.Order.GetAll
{
    public class GetAllOrdersQueryRequest : PaginationRequestParams, IRequest<GetAllOrdersQueryResponse>
    {
    }
}
