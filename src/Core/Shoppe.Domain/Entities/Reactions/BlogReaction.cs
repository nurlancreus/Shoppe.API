using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Reactions
{
    public class BlogReaction : Reaction
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
        public BlogReactionType BlogReactionType { get; set; }
    }
}
