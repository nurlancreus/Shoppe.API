using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Contact
{
    public record AnswerContactMessageDTO
    {
        public Guid ContactId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
