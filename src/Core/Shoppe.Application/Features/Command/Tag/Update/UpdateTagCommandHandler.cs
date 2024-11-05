using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.Features.Command.Tag.Create;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Tag.Update
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommandRequest, UpdateTagCommandResponse>
    {
        private readonly ITagService _tagService;

        public UpdateTagCommandHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<UpdateTagCommandResponse> Handle(UpdateTagCommandRequest request, CancellationToken cancellationToken)
        {

            await _tagService.UpdateAsync(new UpdateTagDTO
            {
                Id = (Guid)request.Id!,
                Name = request.Name,
                Description = request.Description,
            }, cancellationToken);

            return new UpdateTagCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
