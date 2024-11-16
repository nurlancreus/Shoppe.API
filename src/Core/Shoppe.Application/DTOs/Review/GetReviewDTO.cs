using Shoppe.Application.DTOs.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Review
{
    public record GetReviewDTO
    {
        public Guid Id { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public GetImageFileDTO? ProfilePhoto { get; set; }
        public string? Body { get; set; }
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
