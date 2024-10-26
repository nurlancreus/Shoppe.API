using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Review.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommandRequest, CreateReviewCommandResponse>
    {
        private readonly IReviewService _reviewService;

        public CreateReviewCommandHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<CreateReviewCommandResponse> Handle(CreateReviewCommandRequest request, CancellationToken cancellationToken)
        {
            await _reviewService.CreateAsync(new CreateReviewDTO
            {
                Body = request.Body,
                Rating = request.Rating,
            }, request.EntityId!, request.Type, cancellationToken);

            return new CreateReviewCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.AddedSuccessMessage("Review")
            };
        }
    }
}
