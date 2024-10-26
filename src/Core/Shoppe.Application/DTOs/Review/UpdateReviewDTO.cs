using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Review
{
    public record UpdateReviewDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Body { get; set; }
        public int? Rating { get; set; }
    }
}
