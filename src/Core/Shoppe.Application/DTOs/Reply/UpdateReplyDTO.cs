using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Reply
{
    public record UpdateReplyDTO
    {
        public Guid Id { get; set; }
        public string? Body { get; set; }
    }
}
