using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetAllReviews
{
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQueryRequest, GetAllReviewsQueryResponse>
    {
        private readonly IReviewService _reviewService;

        public GetAllReviewsQueryHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<GetAllReviewsQueryResponse> Handle(GetAllReviewsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _reviewService.GetAllReviewsAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllReviewsQueryResponse()
            {
                IsSuccess = true,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Reviews,
            };
        }
    }
}
