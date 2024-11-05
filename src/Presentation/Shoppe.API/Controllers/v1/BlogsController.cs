using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Features.Command.Blog.ChangeCover;
using Shoppe.Application.Features.Command.Blog.Create;
using Shoppe.Application.Features.Command.Blog.Delete;
using Shoppe.Application.Features.Command.Blog.RemoveImage;
using Shoppe.Application.Features.Command.Blog.Update;
using Shoppe.Application.Features.Command.Reply.Create;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Query.Blog.Get;
using Shoppe.Application.Features.Query.Blog.GetAll;
using Shoppe.Application.Features.Query.Files.GetAllImageFiles;
using Shoppe.Application.Features.Query.Reply.GetRepliesByEntity;
using Shoppe.Application.Features.Query.Review.GetReviewByEntity;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Enums;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class BlogsController : ApplicationControllerBase
    {
        private readonly ISender _sender;

        public BlogsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBlogCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllBlogsQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetBlogByIdQueryRequest
            {
                BlogId = id
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateBlogCommandRequest request)
        {
            request.BlogId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteBlogCommandRequest
            {
                BlogId = id
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{BlogId}/replies")]
        public async Task<IActionResult> GetReplies(Guid BlogId)
        {
            var request = new GetRepliesByEntityQueryRequest { EntityId = BlogId, ReplyType = ReplyType.Blog };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPost("{BlogId}/replies")]
        public async Task<IActionResult> AddReply(Guid BlogId, [FromBody] CreateReplyCommandRequest request)
        {
            request.Type = ReplyType.Blog;
            request.EntityId = BlogId;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{blogId}/change-cover")]
        public async Task<IActionResult> ChangeCoverImage(Guid blogId, [FromForm] ChangeCoverCommandRequest request)
        {
            request.BlogId = blogId;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{blogId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(Guid blogId, Guid imageId)
        {
            var request = new RemoveBlogImageCommandRequest
            {
                BlogId = blogId,
                ImageId = imageId
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetAllBlogsImages([FromQuery] GetAllImageFIlesQueryRequest request)
        {
            request.Type = ImageFileType.Blog;

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
