using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Review.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommandRequest, DeleteReviewCommandResponse>
    {
        private readonly IReviewService _reviewService;

        public DeleteReviewCommandHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<DeleteReviewCommandResponse> Handle(DeleteReviewCommandRequest request, CancellationToken cancellationToken)
        {
            await _reviewService.DeleteAsync(request.Id!, cancellationToken);

            return new DeleteReviewCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.DeletedSuccessMessage("Review")
            };
        }
    }
}
