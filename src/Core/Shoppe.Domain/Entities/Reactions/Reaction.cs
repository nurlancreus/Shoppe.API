using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Reactions
{
    public class Reaction : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public string EntityType { get; set; } = string.Empty; // Type of the entity (Reply or Blog)

    }
}
