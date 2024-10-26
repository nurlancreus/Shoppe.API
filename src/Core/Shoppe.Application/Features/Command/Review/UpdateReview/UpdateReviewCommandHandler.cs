using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Review.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommandRequest, UpdateReviewCommandResponse>
    {
        private readonly IReviewService _reviewService;

        public UpdateReviewCommandHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<UpdateReviewCommandResponse> Handle(UpdateReviewCommandRequest request, CancellationToken cancellationToken)
        {
            await _reviewService.UpdateAsync(request.ToUpdateReviewDTO(), cancellationToken);

            return new UpdateReviewCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.UpdatedSuccessMessage("Review")
            };
        }
    }
}
