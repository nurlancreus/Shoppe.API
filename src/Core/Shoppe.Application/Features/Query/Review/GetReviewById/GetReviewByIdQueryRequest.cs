using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetReviewById
{
    public class GetReviewByIdQueryRequest : IRequest<GetReviewByIdQueryResponse>
    {
        public Guid? Id { get; set; }
    }
}
