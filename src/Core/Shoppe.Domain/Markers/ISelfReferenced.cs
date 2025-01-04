using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Replies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Markers
{
    public interface ISelfReferenced<T> where T : IBase
    {
        public Guid? ParentId { get; set; }
        public T? Parent { get; set; }

        public ICollection<T> Children { get; set; }

    }
}
