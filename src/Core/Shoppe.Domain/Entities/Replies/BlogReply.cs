using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Replies
{
    public class BlogReply : Reply
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
    }
}
