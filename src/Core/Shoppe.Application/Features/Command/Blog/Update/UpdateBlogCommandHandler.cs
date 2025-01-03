﻿using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Update
{
    public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommandRequest, UpdateBlogCommandResponse>
    {
        private readonly IBlogService _blogService;

        public UpdateBlogCommandHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<UpdateBlogCommandResponse> Handle(UpdateBlogCommandRequest request, CancellationToken cancellationToken)
        {
            await _blogService.UpdateAsync(new UpdateBlogDTO
            {
                BlogId = request.BlogId,
                Title = request.Title,
                Categories = request.Categories,
                Tags = request.Tags,
                Content = request.Content,
                ContentImages = request.ContentImages,
            }, cancellationToken);

            return new UpdateBlogCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
