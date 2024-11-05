using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reply.Create
{
    public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommandRequest, CreateReplyCommandResponse>
    {
        private readonly IReplyService _replyService;

        public CreateReplyCommandHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<CreateReplyCommandResponse> Handle(CreateReplyCommandRequest request, CancellationToken cancellationToken)
        {
            await _replyService.CreateAsync(new CreateReplyDTO
            {
                Body = request.Body
            }, (Guid)request.EntityId!, request.Type, cancellationToken);

            return new CreateReplyCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
