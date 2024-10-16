using Microsoft.AspNetCore.Identity;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser, IBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public ICollection<Blog> Blogs { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
