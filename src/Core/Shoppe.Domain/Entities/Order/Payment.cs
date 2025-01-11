using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public PaymentMethod Method { get; set; }
        public Guid OrderId { get; set; } 
        public Order Order { get; set; } = null!;

        public double Amount { get; set; } 
        public PaymentStatus Status { get; set; } 
        public string Reference { get; set; } = string.Empty;  
        public string TransactionId { get; set; } = string.Empty;  
    }
}
