using MediatR;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetReviewByEntity
{
    public class GetReviewsByEntityRequest : IRequest<GetReviewsByEntityResponse>
    {
        public string? EntityId { get; set; }
        public ReviewType ReviewType { get; set; } = ReviewType.Product;
    }
}
