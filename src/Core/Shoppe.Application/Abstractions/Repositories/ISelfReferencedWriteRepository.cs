using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories
{
    public interface ISelfReferencedWriteRepository<T> : IWriteRepository<T> where T : BaseEntity, ISelfReferenced<T>
    {
        bool RecursiveDelete(T entity);

    }
}
