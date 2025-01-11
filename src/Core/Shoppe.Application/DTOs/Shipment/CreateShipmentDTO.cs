using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Shipment
{
    public record CreateShipmentDTO
    {
        public string Provider { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
    }
}
