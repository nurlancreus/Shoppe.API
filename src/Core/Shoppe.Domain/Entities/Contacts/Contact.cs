using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Contacts
{
    public class Contact : BaseEntity
    {
        public string Type { get; set; }
        public ContactSubject Subject { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsAnswered { get; set; }
        public DateTime? AnsweredAt { get; set; }
    }
}
