using MediatR;
using Shoppe.Application.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetAllReviews
{
    public class GetAllReviewsQueryRequest : PaginationRequestParams, IRequest<GetAllReviewsQueryResponse>
    {
    }
}
