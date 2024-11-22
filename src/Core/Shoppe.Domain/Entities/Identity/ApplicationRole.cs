using Microsoft.AspNetCore.Identity;
using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole, IBase
    {
        public string? Description { get; set; }
        //public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
        //public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
