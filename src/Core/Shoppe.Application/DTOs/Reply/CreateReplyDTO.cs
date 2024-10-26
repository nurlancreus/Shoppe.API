using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Reply
{
    public record CreateReplyDTO
    {
        public string Body { get; set; } = string.Empty;
    }
}
