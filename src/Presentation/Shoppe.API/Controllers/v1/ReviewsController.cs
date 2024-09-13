using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Command.Review.DeleteReview;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using Shoppe.Application.Features.Query.Review.GetAllReviews;
using Shoppe.Application.Features.Query.Review.GetReviewById;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class ReviewsController : ApplicationControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllReviewsQueryRequest getAllReviewsQueryRequest)
        {
            var response = await _mediator.Send(getAllReviewsQueryRequest);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var getReviewByIdRequest = new GetReviewByIdQueryRequest { Id = id };

            var response = await _mediator.Send(getReviewByIdRequest);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewCommandRequest createReviewCommandRequest)
        {

            var response = await _mediator.Send(createReviewCommandRequest);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateReviewCommandRequest updateReviewCommandRequest)
        {
            updateReviewCommandRequest.Id = id;

            var response = await _mediator.Send(updateReviewCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteReviewCommandRequest = new DeleteReviewCommandRequest { Id = id };

            var response = await _mediator.Send(deleteReviewCommandRequest);

            return Ok(response);
        }
    }
}
