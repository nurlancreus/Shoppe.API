using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetProductReviews
{
    public class GetProductReviewsQueryResponse : AppResponseWithData<List<GetReviewDTO>>
    {
    }
}
