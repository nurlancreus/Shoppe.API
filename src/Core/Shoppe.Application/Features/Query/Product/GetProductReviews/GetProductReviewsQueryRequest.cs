using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Product.GetProductReviews
{
    public class GetProductReviewsQueryRequest : IRequest<GetProductReviewsQueryResponse>
    {
        public string? ProductId { get; set; }
    }
}
