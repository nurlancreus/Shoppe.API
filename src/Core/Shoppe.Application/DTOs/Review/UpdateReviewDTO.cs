using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Review
{
    public record UpdateReviewDTO
    {
        public Guid Id { get; set; }
        public string? Body { get; set; }
        public int? Rating { get; set; }
    }
}
