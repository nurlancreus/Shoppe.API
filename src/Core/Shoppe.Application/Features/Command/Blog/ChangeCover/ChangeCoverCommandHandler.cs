using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.ChangeCover
{
    public class ChangeCoverCommandHandler : IRequestHandler<ChangeCoverCommandRequest, ChangeCoverCommandResponse>
    {
        private readonly IBlogService _blogService;

        public ChangeCoverCommandHandler(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<ChangeCoverCommandResponse> Handle(ChangeCoverCommandRequest request, CancellationToken cancellationToken)
        {
            await _blogService.ChangeCoverImageAsync((Guid)request.BlogId!, request.NewImageId, request.NewImageFile, cancellationToken);

            return new ChangeCoverCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
