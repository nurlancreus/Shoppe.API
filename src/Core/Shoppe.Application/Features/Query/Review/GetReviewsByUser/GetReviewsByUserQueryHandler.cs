using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetReviewByUser
{
    public class GetReviewsByUserQueryHandler : IRequestHandler<GetReviewsByUserQueryRequest, GetReviewsByUserQueryResponse>
    {
        private readonly IReviewService _reviewService;

        public GetReviewsByUserQueryHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<GetReviewsByUserQueryResponse> Handle(GetReviewsByUserQueryRequest request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetReviewsByUserAsync(request.UserId!, cancellationToken);

            return new GetReviewsByUserQueryResponse
            {
                IsSuccess = true,
                Data = reviews
            };
        }
    }
}
