using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Reaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reaction.ToggleReaction
{
    public class ToggleReactionCommandHandler : IRequestHandler<ToggleReactionCommandRequest, ToggleReactionCommandResponse>
    {
        private readonly IReactionService _reactionService;

        public ToggleReactionCommandHandler(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        public async Task<ToggleReactionCommandResponse> Handle(ToggleReactionCommandRequest request, CancellationToken cancellationToken)
        {
            await _reactionService.ToggleReactionAsync(new ToggleReactionDTO
            {
                BlogReactionType = request.BlogReactionType,
                EntityId = request.EntityId,
                EntityType = request.EntityType,
                ReplyReactionType = request.ReplyReactionType   
            }, cancellationToken);

            return new ToggleReactionCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
