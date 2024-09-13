using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Review
{
    public class UpdateReviewDTO
    {
        public string Id { get; set; } = null!;
        public string? Body { get; set; }
        public int? Rating { get; set; }
    }
}
