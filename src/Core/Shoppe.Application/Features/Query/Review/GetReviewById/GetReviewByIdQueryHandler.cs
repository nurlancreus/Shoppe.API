using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetReviewById
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQueryRequest, GetReviewByIdQueryResponse>
    {
        private readonly IReviewService _reviewService;

        public GetReviewByIdQueryHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<GetReviewByIdQueryResponse> Handle(GetReviewByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var review = await _reviewService.GetReviewAsync(request.Id, cancellationToken);

            return new GetReviewByIdQueryResponse()
            {
                IsSuccess = true,
                Data = review
            };
        }
    }
}
