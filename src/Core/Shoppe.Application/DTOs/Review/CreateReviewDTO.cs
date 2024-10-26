using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Review
{
    public class CreateReviewDTO
    {
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        //public string? Email { get; set; }
        public string? Body { get; set; } = null!;
       // public bool? SaveMe { get; set; }
        public int Rating { get; set; }
    }
}
