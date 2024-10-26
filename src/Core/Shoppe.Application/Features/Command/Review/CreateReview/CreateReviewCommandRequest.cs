using MediatR;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Review.CreateReview
{
    public class CreateReviewCommandRequest : IRequest<CreateReviewCommandResponse>
    {
        public string? Body { get; set; } = null!;
        // public bool? SaveMe { get; set; }
        public int Rating { get; set; }
        public string? EntityId { get; set; } = null!;
        public ReviewType Type { get; set; } = ReviewType.Product;
    }
}
