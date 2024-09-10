using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        bool Update(T entity);
        bool Delete(T entity);
        bool DeleteRange(IEnumerable<T> entities);
    }
}
