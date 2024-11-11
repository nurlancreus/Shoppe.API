using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Create
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommandRequest, CreateBlogCommandResponse>
    {
        private readonly IBlogService _blogService;

        public CreateBlogCommandHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<CreateBlogCommandResponse> Handle(CreateBlogCommandRequest request, CancellationToken cancellationToken)
        {
            await _blogService.CreateAsync(new CreateBlogDTO
            {
                Title = request.Title,
                Categories = request.Categories,
                Tags = request.Tags,
                CoverImageFile = request.CoverImageFile,
                Content = request.Content,
                ContentImages = request.ContentImages,
            }, cancellationToken);

            return new CreateBlogCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
