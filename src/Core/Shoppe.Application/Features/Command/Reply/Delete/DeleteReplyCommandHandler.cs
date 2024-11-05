using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reply.Delete
{
    public class DeleteReplyCommandHandler : IRequestHandler<DeleteReplyCommandRequest, DeleteReplyCommandResponse>
    {
        private readonly IReplyService _replyService;

        public DeleteReplyCommandHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<DeleteReplyCommandResponse> Handle(DeleteReplyCommandRequest request, CancellationToken cancellationToken)
        {
            await _replyService.DeleteAsync((Guid)request.Id!, cancellationToken);

            return new DeleteReplyCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
