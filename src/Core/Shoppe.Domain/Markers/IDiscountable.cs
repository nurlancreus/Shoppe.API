using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Markers
{
    public interface IDiscountable
    {
        public Guid? DiscountId { get; set; }
        public Discount? Discount { get; set; }
    }
}
