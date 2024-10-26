using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Query.Review.GetAllReviews;
using Shoppe.Application.Features.Query.Review.GetReviewById;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using Shoppe.Application.Features.Command.Review.DeleteReview;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class ReviewsController : ApplicationControllerBase
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
        public async Task<IActionResult> Get(string id)
        {
            var request = new GetReviewByIdQueryRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateReviewCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteReviewCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
