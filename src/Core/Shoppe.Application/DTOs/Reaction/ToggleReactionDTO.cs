using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Reaction
{
    public record ToggleReactionDTO
    {
        public Guid EntityId { get; set; }
        public ReactionEntityType? EntityType { get; set; }
        public ReplyReactionType? ReplyReactionType { get; set; }
        public BlogReactionType? BlogReactionType { get; set; }
    }
}
