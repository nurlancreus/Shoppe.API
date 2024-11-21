using MediatR;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reaction.ToggleReaction
{
    public class ToggleReactionCommandRequest : IRequest<ToggleReactionCommandResponse>
    {
        public Guid EntityId { get; set; }
        public ReactionEntityType? EntityType { get; set; }
        public string? ReplyReactionType { get; set; }
        public string? BlogReactionType { get; set; }
    }
}
