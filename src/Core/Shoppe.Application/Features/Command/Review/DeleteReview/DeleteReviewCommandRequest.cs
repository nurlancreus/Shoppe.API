using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Review.DeleteReview
{
    public class DeleteReviewCommandRequest : IRequest<DeleteReviewCommandResponse>
    {
        public Guid? Id { get; set; } = null!;
    }
}
