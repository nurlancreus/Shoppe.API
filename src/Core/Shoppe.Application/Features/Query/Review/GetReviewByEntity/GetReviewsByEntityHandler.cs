using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetReviewByEntity
{
    public class GetReviewsByEntityHandler : IRequestHandler<GetReviewsByEntityRequest, GetReviewsByEntityResponse>
    {
        private readonly IReviewService _reviewService;

        public GetReviewsByEntityHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<GetReviewsByEntityResponse> Handle(GetReviewsByEntityRequest request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetReviewsByEntityAsync((Guid)request.EntityId!, request.ReviewType, cancellationToken);

            return new GetReviewsByEntityResponse
            {
                IsSuccess = true,
                Data = reviews,
            };
        }
    }
}
