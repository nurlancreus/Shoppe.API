using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Checkout
{
    public record GetCheckoutResponseDTO
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string? ApprovalUrl { get; set; }
        public string? ClientSecret { get; set; }
        public string? Message { get; set; }
    }
}
