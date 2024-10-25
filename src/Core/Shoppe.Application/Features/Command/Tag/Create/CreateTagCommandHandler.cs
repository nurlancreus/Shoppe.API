using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Tag.Create
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommandRequest, CreateTagCommandResponse>
    {
        private readonly ITagService _tagService;

        public CreateTagCommandHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<CreateTagCommandResponse> Handle(CreateTagCommandRequest request, CancellationToken cancellationToken)
        {
            TagType tagType = Enum.TryParse(request.Type, out TagType parsedType) ? parsedType : TagType.Blog;

            await _tagService.CreateAsync(new CreateTagDTO
            {
                Name = request.Name,
                Description = request.Description,
                Type = tagType
            }, cancellationToken);

            return new CreateTagCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
