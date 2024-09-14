using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Body { get; set; } = null!;
        public Rating Rating { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? Reviewer { get; set; }
    }
}
