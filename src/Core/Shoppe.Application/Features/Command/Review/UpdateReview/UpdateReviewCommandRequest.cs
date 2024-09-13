using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Review.UpdateReview
{
    public class UpdateReviewCommandRequest : IRequest<UpdateReviewCommandResponse>
    {
        public string? Id { get; set; }
        public string? Body { get; set; }
        public int? Rating { get; set; }
    }
}
