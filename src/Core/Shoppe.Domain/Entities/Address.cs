using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;
    }
}
