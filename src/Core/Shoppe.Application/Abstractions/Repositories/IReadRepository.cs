using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<IQueryable<T>> GetAllAsync(bool isTracking = true);
        Task<IQueryable<T>> GetAllWhereAsync(Expression<Func<T, bool>> method, bool isTracking = true);
        Task<T?> GetByIdAsync(string id, bool isTracking = true);
        Task<T?> GetAsync(Expression<Func<T, bool>> method, bool isTracking = true);
    }
}
