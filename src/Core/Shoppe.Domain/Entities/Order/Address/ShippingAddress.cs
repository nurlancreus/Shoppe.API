using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class ShippingAddress : Address
    {
        [ForeignKey(nameof(Account))]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser Account { get; set; } = null!;
    }
}
