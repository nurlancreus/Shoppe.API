using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Query.Review.GetAllReviews;
using Shoppe.Application.Features.Query.Review.GetReviewById;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using Shoppe.Application.Features.Command.Review.DeleteReview;
using Microsoft.AspNetCore.Authorization;

namespace Shoppe.API.Controllers.v1
{
    [Authorize]
    public class ReviewsController : ApplicationVersionController
    {
        private readonly ISender _sender;

        public ReviewsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllReviewsQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetReviewByIdQueryRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteReviewCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
