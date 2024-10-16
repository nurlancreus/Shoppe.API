using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Reviews
{
    public class ProductReview : Review
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
