using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetAllReviews
{
    public class GetAllReviewsQueryResponse : AppResponseWithPaginatedData<List<GetReviewDTO>>
    {
    }
}
