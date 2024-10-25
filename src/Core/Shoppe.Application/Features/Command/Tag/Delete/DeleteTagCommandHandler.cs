using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Tag.Delete
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommandRequest, DeleteTagCommandResponse>
    {
        private readonly ITagService _tagService;

        public DeleteTagCommandHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<DeleteTagCommandResponse> Handle(DeleteTagCommandRequest request, CancellationToken cancellationToken)
        {
            await _tagService.DeleteAsync(request.Id!, cancellationToken);

            return new DeleteTagCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
