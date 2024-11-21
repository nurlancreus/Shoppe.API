using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Reaction;
using Shoppe.Domain.Enums;
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
            var toggleReactionDTO = new ToggleReactionDTO
            {
                EntityId = request.EntityId,
                EntityType = request.EntityType,
            };

            if (request.BlogReactionType != null)
            {
                toggleReactionDTO.BlogReactionType = Enum.Parse<BlogReactionType>(request.BlogReactionType!);
            }
            else if (request.ReplyReactionType != null)
            {
                toggleReactionDTO.ReplyReactionType = Enum.Parse<ReplyReactionType>(request.ReplyReactionType!);
            }

            await _reactionService.ToggleReactionAsync(toggleReactionDTO, cancellationToken);

            return new ToggleReactionCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
