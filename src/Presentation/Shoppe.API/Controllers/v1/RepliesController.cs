using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Reaction.ToggleReaction;
using Shoppe.Application.Features.Command.Reply.Create;
using Shoppe.Application.Features.Command.Reply.Delete;
using Shoppe.Application.Features.Command.Reply.Update;
using Shoppe.Application.Features.Command.Review.DeleteReview;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using Shoppe.Application.Features.Query.Reaction.GetReplyReactions;
using Shoppe.Application.Features.Query.Reply.Get;
using Shoppe.Application.Features.Query.Reply.GetAll;
using Shoppe.Application.Features.Query.Reply.GetRepliesByEntity;
using Shoppe.Application.Features.Query.Review.GetAllReviews;
using Shoppe.Application.Features.Query.Review.GetReviewById;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;

namespace Shoppe.API.Controllers.v1
{
    public class RepliesController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public RepliesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllRepliesQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("{id}/replies")]
        public async Task<IActionResult> AddReply([FromRoute] Guid id, [FromBody] CreateReplyCommandRequest request)
        {
            request.Type = ReplyType.Reply;
            request.EntityId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetReplyByIdQueryRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{ReplyId}/replies")]
        public async Task<IActionResult> GetReplies(Guid ReplyId)
        {
            var request = new GetRepliesByEntityQueryRequest { EntityId = ReplyId, ReplyType = ReplyType.Reply };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReplyCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteReplyCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}/reactions")]
        public async Task<IActionResult> GetReactions(Guid id)
        {
            var request = new GetReplyReactionsQueryRequest { ReplyId = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("{id}/reactions")]
        public async Task<IActionResult> ToggleReaction(Guid id, ToggleReactionCommandRequest request)
        {
            request.EntityId = id;
            request.EntityType = ReactionEntityType.Reply;

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
