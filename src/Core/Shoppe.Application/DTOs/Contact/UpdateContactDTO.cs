using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Contact
{
    public record UpdateContactDTO
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public ContactSubject? Subject { get; set; }
        public string? Message { get; set; }
    }
}
