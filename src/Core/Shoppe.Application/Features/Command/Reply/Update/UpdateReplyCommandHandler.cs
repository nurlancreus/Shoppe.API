using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reply.Update
{
    public class UpdateReplyCommandHandler : IRequestHandler<UpdateReplyCommandRequest, UpdateReplyCommandResponse>
    {
        private readonly IReplyService _replyService;

        public UpdateReplyCommandHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<UpdateReplyCommandResponse> Handle(UpdateReplyCommandRequest request, CancellationToken cancellationToken)
        {
            await _replyService.UpdateAsync(new UpdateReplyDTO
            {
                Id = request.Id!,
                Body = request.Body,
            }, cancellationToken);

            return new UpdateReplyCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
