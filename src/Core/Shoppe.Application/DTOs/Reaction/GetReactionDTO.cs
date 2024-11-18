using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Reaction
{
    public record GetReactionDTO
    {
        public Guid Id { get; set; }
        public bool IsToggled { get; set; }
        public int ReactionCount { get; set; }
        public string ReactionType { get; set;} = string.Empty;
    }
}
