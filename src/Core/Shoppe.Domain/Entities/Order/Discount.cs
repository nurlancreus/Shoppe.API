using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DiscountProduct> ProductMappings { get; set; } = [];
        public ICollection<DiscountCategory> CategoryMappings { get; set; } = [];

    }

}
