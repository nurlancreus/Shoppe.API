using Shoppe.Domain.Entities.Base;
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
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool SaveMe { get; set; } 
        public Rating Rating { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
