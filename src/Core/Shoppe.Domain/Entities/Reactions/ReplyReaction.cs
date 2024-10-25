using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Reactions
{
    public class ReplyReaction : Reaction
    {
        public Guid ReplyId { get; set; }
        public Reply Reply { get; set; } = null!;
        public ReplyReactionType ReplyReactionType { get; set; }
    }
}
